using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

using RawENGINE;
using System;
using Microsoft.Xna.Framework.Input;

using Ini;

namespace SquareBall
{
    public class Field : DrawableGameComponent
    {


        SpriteBatch spriteBatch;
        Texture2D FieldTexture;
        Texture2D BackTexture;
        Texture2D ShadowTexture;
        double Scale = 1;

        public int BlueScore = 0;
        public int OrangeScore = 0;

        Point RightGoalOrigin;
        int Goalradius;
        Point LeftGoalOrigin;

        PlayerController playerController;
        PlayerController enemyController;

        public bool P1 = GameSettings.getBool("Players.MousePlayer");
        public bool P2 = GameSettings.getBool("Players.KeyPlayer1");
        public bool P3 = GameSettings.getBool("Players.KeyPlayer2");
        public bool P4 = GameSettings.getBool("Players.Enemy");
        
        Cursor cursor;
        Texture2D cursorTex;

        Ball ball;
        Counter goalC = new Counter(100, 1);

        public int Seconds = 0;
        public int Minutes = 0;

        public Field(SpriteBatch sb, Game game) : base(game)
        {
            spriteBatch = sb;

            //Load Textures
            {
                using (Stream stream = TitleContainer.OpenStream("Content/Field.png"))
                {
                    FieldTexture = Texture2D.FromStream(game.GraphicsDevice, stream);
                }
                using (Stream stream = TitleContainer.OpenStream("Content/Back.png"))
                {
                    BackTexture = Texture2D.FromStream(game.GraphicsDevice, stream);
                }
                using (Stream stream = TitleContainer.OpenStream("Content/Cursor.png"))
                {
                    cursorTex = Texture2D.FromStream(game.GraphicsDevice, stream);
                }
                using (Stream stream = TitleContainer.OpenStream("Content/Shadow.png"))
                {
                    ShadowTexture = Texture2D.FromStream(game.GraphicsDevice, stream);
                }
            }
            //Calculate Scale and Borders
            {
                if (FieldTexture.Bounds.Height * Scale >= SBGame.ScreenH / (1.3) || FieldTexture.Bounds.Width * Scale >= SBGame.ScreenW)
                    do
                    {
                        Scale -= 0.1;
                    }
                    while (FieldTexture.Bounds.Height * Scale >= SBGame.ScreenH / (1.3) || FieldTexture.Bounds.Width * Scale >= SBGame.ScreenW);

                SBGame.LeftM = (int)(SBGame.ScreenW - FieldTexture.Bounds.Width * Scale) / 2;
                SBGame.TopM = (int)(SBGame.ScreenH - FieldTexture.Bounds.Height * Scale) / 2;
                SBGame.Scale = Scale;
            }

            RightGoalOrigin = new Point(1100, 300);
            LeftGoalOrigin = new Point(-100, 300);
            Goalradius = 200;

            ball = new Ball(game, sb, (FieldTexture.Bounds.Width), (FieldTexture.Bounds.Height));
            ball.SetGoalInfo(RightGoalOrigin, LeftGoalOrigin, (Goalradius - 10));

            if (P1)
            {
                playerController = new PlayerController();
                playerController.Add(new Player(game, sb, ball, 73, 43, 306, 188));
                playerController.Add(new Player(game, sb, ball, 73, 558, 306, 417));
                playerController.Add(new GoalKeeper(game, sb, ball, -100, 300, -Goalradius - 50));
                playerController.GiveBall();
            }
            if (P2)
            {
                playerController = new PlayerIIController();
                playerController.Add(new KPlayer(game, sb, ball, 694, 188, 927, 43));
                playerController.Add(new KPlayer(game, sb, ball, 694, 417, 927, 558));
                playerController.Add(new KGoalKeeper(game, sb, ball, 1100, 300, Goalradius + 50));
                playerController.GiveBall();
            }
            if (P3)
            {
                enemyController = new PlayerIController();
                playerController.Add(new KPlayer(game, sb, ball, 73, 43, 306, 188));
                playerController.Add(new KPlayer(game, sb, ball, 73, 558, 306, 417));
                playerController.Add(new KGoalKeeper(game, sb, ball, -100, 300, -Goalradius - 50));
            }
            if (P4)
            {
                enemyController = new EnemyController(playerController);
                enemyController.Add(new Enemy(game, sb, ball, 73, 43, 306, 188));
                enemyController.Add(new Enemy(game, sb, ball, 73, 558, 306, 417));
                enemyController.Add(new EvilGoalkeeper(game, sb, ball, -100, 300, -Goalradius - 50));
            }

            cursor = new Cursor(game, cursorTex, sb);
        }

        bool Goal;
        byte GoalB;

        public override void Update(GameTime gameTime)
        {
            if (Seconds == 0)
            {
                Seconds = gameTime.TotalGameTime.Seconds;
                Minutes = gameTime.TotalGameTime.Minutes;
            }

            if (!Goal)
            {
                    playerController.Update(gameTime, ball);
                    enemyController.Update(gameTime, ball);
            }

            if (!Goal && ball.isActive && ball.Velocity < 1.8f)
                ball.Stop();
            if (!Goal && ball.isActive && ball.Velocity < 0.1f)
            {
                ball.Deactivate();
                if (playerController.LastStrike > enemyController.LastStrike)
                    enemyController.GiveBall();
                else playerController.GiveBall();
            }

            if (!goalC.Running)
            GoalB = ball.CheckGoal();
            if (GoalB != 0 && !Goal)
                Goal = true;

            if (Goal)
            {
                if (goalC.Add())
                {
                    ball.Deactivate();
                    if (GoalB == 1)
                    {
                        playerController.GiveBall();
                        BlueScore++;
                    }
                    if (GoalB == 2)
                    {
                        enemyController.GiveBall();
                        OrangeScore++;
                    }
                    goalC.Reset();
                    Goal = false;
                }
                else ball.Stop();
            }

            ball.Update(gameTime);

            cursor.Update(gameTime);

           
            base.Update(gameTime);
        }

        private Rectangle Re(double x0, double y0, double width, double height)
        {
            return new Rectangle((int)(x0), (int)(y0 + 50), (int)(width * Scale), (int)(height * Scale));
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(FieldTexture, Re(SBGame.LeftM, SBGame.TopM, 1000, 600), Color.White);

            enemyController.Draw(gameTime);
            playerController.Draw(gameTime);

            ball.Draw(gameTime);

            spriteBatch.Draw(BackTexture, Re(-300 * Scale + SBGame.LeftM, -200 * Scale + SBGame.TopM, 1600, 1000), Color.White);
            spriteBatch.Draw(ShadowTexture, Re(SBGame.LeftM, SBGame.TopM, 1000, 600), Color.White);
            cursor.Draw(gameTime);
          
            int s = 60 - gameTime.TotalGameTime.Seconds - Seconds;
            int m = 4 - gameTime.TotalGameTime.Minutes - Minutes;

            // Write Text

            SBGame.Font.DrawString(spriteBatch, (m).ToString() + ":" + s.ToString(),
                new Vector2(SBGame.ScreenW/2 - 10*4, 65), new Color(255, 255, 255), null, 1);

            SBGame.Font.DrawString(spriteBatch, BlueScore.ToString(),
                new Vector2(SBGame.ScreenW / 2 - 10*7, 10), new Color(0, 90, 255), null, 1);
            SBGame.Font.DrawString(spriteBatch, ":",
                new Vector2(SBGame.ScreenW / 2 + 65 - 10*7, 10), Color.White, null, 1);
            SBGame.Font.DrawString(spriteBatch, OrangeScore.ToString(),
                new Vector2(SBGame.ScreenW / 2 + 100 - 10*7, 10), new Color(255, 115, 0), null, 1);
            base.Draw(gameTime);
        }
    }
}
