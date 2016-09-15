using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using SquareBall.Ways;

namespace SquareBall
{
    public class KPlayer : Player
    {
        Keys Left, Right, Up, Down, Strike;
        protected Texture2D PlayerTex2;

        public void setKeys(Keys L, Keys R, Keys U, Keys D, Keys S)
        {
            Left = L;
            Right = R;
            Up = U;
            Down = D;
            Strike = S;
        }
        public bool Orange = false;

        public KPlayer(Game game, SpriteBatch sb, Ball _ball, int Sx, int Sy, int Fx, int Fy) : base(game, sb, _ball, Sx, Sy, Fx, Fy)
        {
            Left = Keys.A;
            Right = Keys.D;
            Up = Keys.W;
            Down = Keys.S;
            Strike = Keys.Space;

            Angle = Math.Sign(Position.X - SBGame.ScreenW / 2) * 3.14f/2;

            TexName = "Content/Enemy.png";
            using (Stream stream = TitleContainer.OpenStream(TexName))
            {
                PlayerTex = Texture2D.FromStream(game.GraphicsDevice, stream);
            }
            TexName = "Content/Player.png";
            using (Stream stream = TitleContainer.OpenStream(TexName))
            {
                PlayerTex2 = Texture2D.FromStream(game.GraphicsDevice, stream);
            }
        }


        public override bool ActiveUpdate(GameTime gt)
        {
            Point xy = Mouse.GetState().Position;
            time += gt.ElapsedGameTime.Milliseconds;

            if (Active)
            {
                Vector2 t = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(Left))
                    velocity += -0.25f;
                else if (Keyboard.GetState().IsKeyDown(Right))
                    velocity += 0.25f;

                t = way.GetWayXYbyPointer((float)Position.X + velocity, (int)Position.Y);
                velocity *= 0.9f;

                if (t != Vector2.Zero)
                {
                    t = (t - Position);
                    Position += t;
                }
            }


            if (time > 500 && DirectionChoice && Keyboard.GetState().IsKeyDown(Strike))
            {
                if (Keyboard.GetState().IsKeyDown(Left) && !Keyboard.GetState().IsKeyDown(Right))
                ball.Strike(this, 9, Position, new Vector2((float)Math.Sin(Angle), -(float)Math.Cos(Angle)), -1.7f);
                else if (Keyboard.GetState().IsKeyDown(Right) && !Keyboard.GetState().IsKeyDown(Left))
                    ball.Strike(this, 9, Position, new Vector2((float)Math.Sin(Angle), -(float)Math.Cos(Angle)), 1.7f);
                else if (Keyboard.GetState().IsKeyDown(Right) && Keyboard.GetState().IsKeyDown(Left))
                {
                    Random rnd = new Random();
                    float miss = (rnd.Next(12) - 6) * 3.1415f / 36;
                    ball.Strike(this, 12, Position, new Vector2((float)Math.Sin(Angle + miss), -(float)Math.Cos(Angle + miss)));
                }
                else
                    ball.Strike(this, 10, Position, new Vector2((float)Math.Sin(Angle), -(float)Math.Cos(Angle)));
                LastStrike = gt.TotalGameTime.Seconds;

                Angle = Math.Sign(Position.X - SBGame.ScreenW / 2) * 3.14f / 2; ;
                DirectionChoice = false;
                HasBall = false;
            }

            if (DirectionChoice || (HasBall && Keyboard.GetState().IsKeyDown(Strike)))
            {
                if (!DirectionChoice)
                    time = 0;

                if (Active)
                    Active = false;

                DirectionChoice = true;
                float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;

                // TODO: Add your game logic here.
                Angle -= Math.Sign(Position.Y - SBGame.ScreenH/2)*elapsed*1.3f;
                float circle = MathHelper.Pi * 2;
                Angle = Angle % circle;
                //
            }
            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Active)
            {
                spriteBatch.Draw(Arrow,
                                  new Rectangle((int)Position.X, (int)Position.Y-60, 20, 26),
                                  new Rectangle(0, 0, 41, 113), Color.White,
                                  3.14f, new Vector2(20, 113), SpriteEffects.None, 0);
            }

            if (Orange)
                spriteBatch.Draw(PlayerTex2, Position - new Vector2(PlayerTex.Bounds.Width / 2, PlayerTex.Bounds.Height/ 2), Color.White);
            else
                spriteBatch.Draw(PlayerTex, Position - new Vector2(PlayerTex.Bounds.Width / 2, PlayerTex.Bounds.Height / 2), Color.White);


            if (HasBall)
                spriteBatch.Draw(BallTex,
                    new Rectangle((int)Position.X, (int)Position.Y, 24, 24),
                        Color.White);
            if (DirectionChoice)
            {
                spriteBatch.Draw(Arrow,
                    new Rectangle((int)Position.X, (int)Position.Y, 41, 113),
                    new Rectangle(0, 0, 41, 113), Color.White,
                    Angle, new Vector2(20, 113), SpriteEffects.None, 0);

                if (Keyboard.GetState().IsKeyDown(Left) && !Keyboard.GetState().IsKeyDown(Right))
                     spriteBatch.Draw(Arrow,
                    new Rectangle((int)Position.X, (int)Position.Y, 41, 113),
                    new Rectangle(0, 0, 41, 113), Color.Red,
                    Angle - 3.14f/2, new Vector2(20, 113), SpriteEffects.None, 0);

                else if (Keyboard.GetState().IsKeyDown(Right) && !Keyboard.GetState().IsKeyDown(Left))
                 spriteBatch.Draw(Arrow,
                     new Rectangle((int)Position.X, (int)Position.Y, 41, 113),
                     new Rectangle(0, 0, 41, 113), Color.Red,
                     Angle + 3.14f / 2, new Vector2(20, 113), SpriteEffects.None, 0);

                else if ( Keyboard.GetState().IsKeyDown(Right) && Keyboard.GetState().IsKeyDown(Left))
                 {
                     float angle = (float)Math.Sin(3*Angle) * 3.14f / 5;
                     spriteBatch.Draw(Arrow,
                         new Rectangle((int)Position.X, (int)Position.Y, 27, 75),
                         new Rectangle(0, 0, 41, 113), Color.Red,
                         Angle+angle, new Vector2(20, 113), SpriteEffects.None, 0);
                 }
            }
            //base.Draw(gameTime);
        }

    }
}
