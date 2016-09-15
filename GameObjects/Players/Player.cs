using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using SquareBall.Ways;

namespace SquareBall
{
    public class Player : DrawableGameComponent
    {

        public long LastStrike;

        protected Way way;

        protected Texture2D PlayerTex;
        protected Texture2D BallTex;
        protected float velocity;

        protected string TexName = "Content/Player.png";

        public Vector2 Position;

        public Rectangle getBounds
        {
            get { return new Rectangle(PlayerTex.Bounds.X, PlayerTex.Bounds.X, 20, 20); }
        }

        protected SpriteBatch spriteBatch;

        protected bool Active = false;
        public bool GetActive
        { get { return HasBall; } }
        protected bool HasBall = false;

        protected Ball ball;

        protected bool DirectionChoice = false;
        protected Texture2D Arrow;

        public void Deactivate()
        {
            Active = false;
        }
        public void Activate()
        {
            Active = true;
        }
        public void GetBall()
        {
            HasBall = true;
        }

        public Player(Game game, SpriteBatch sb, Ball _ball, int Sx, int Sy, int Fx, int Fy) : base(game)
        {
            Sx = (int)(Sx * SBGame.Scale) + SBGame.LeftM;
            Fx = (int)(Fx * SBGame.Scale) + SBGame.LeftM;
            Sy = (int)(Sy * SBGame.Scale) + SBGame.TopM + 50;
            Fy = (int)(Fy * SBGame.Scale) + SBGame.TopM + 50;

            way = new Way(Sx, Sy, Fx, Fy);
            spriteBatch = sb;
            ball = _ball;
            using (Stream stream = TitleContainer.OpenStream(TexName))
            {
                PlayerTex = Texture2D.FromStream(game.GraphicsDevice, stream);
            }
            using (Stream stream = TitleContainer.OpenStream("Content/Arrow.png"))
            {
                Arrow = Texture2D.FromStream(game.GraphicsDevice, stream);
            }
            using (Stream stream = TitleContainer.OpenStream("Content/Ball.png"))
            {
                BallTex = Texture2D.FromStream(game.GraphicsDevice, stream);
            }
            Position = new Vector2((Fx+Sx)/2, (Fy+Sy)/2);
        }


        protected int time;

        public bool CheckHover(Point xy)
        {
            return (
                xy.X >= Position.X - PlayerTex.Bounds.Width / 2
            &&  xy.X <= Position.X + PlayerTex.Bounds.Width / 2)
            && (xy.Y >= Position.Y - PlayerTex.Bounds.Height / 2 
            &&  xy.Y <= Position.Y + PlayerTex.Bounds.Height / 2);
        }

        public virtual bool ActiveUpdate(GameTime gt)
        {
            Point xy = Mouse.GetState().Position;
            time += gt.ElapsedGameTime.Milliseconds;

            if (Active)
            {
                {
                    Vector2 t = way.GetWayXYbyPointer(xy.X, xy.Y);
                    if (t != Vector2.Zero)
                    {
                        t = (t - Position);

                        Position += t;
                        
                    }
                }
            }

            if (time > 1000 && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (CheckHover(xy))
                {
                    Active = !Active;
                    time = 0;
                    return true;
                }
            }

            if (time > 500 && DirectionChoice && Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                ball.Strike(this, 7, Position, new Vector2((float)Math.Sin(Angle), -(float)Math.Cos(Angle)), 1.1f);
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    ball.Strike(this, 7, Position, new Vector2((float)Math.Sin(Angle), -(float)Math.Cos(Angle)), -1.1f);
                else if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    Random rnd = new Random();
                    float miss = (rnd.Next(12) - 6) * 3.1415f / 36;
                    ball.Strike(this, 11, Position, new Vector2((float)Math.Sin(Angle + miss), -(float)Math.Cos(Angle + miss)));
                }
                else
                    ball.Strike(this, 8, Position, new Vector2((float)Math.Sin(Angle), -(float)Math.Cos(Angle)));
                LastStrike = gt.TotalGameTime.Seconds;
                DirectionChoice = false;
                HasBall = false;
            }

            if (DirectionChoice || (HasBall && Keyboard.GetState().IsKeyDown(Keys.S)))
            {
                if (!DirectionChoice)
                    time = 0;

                if (Active)
                    Active = false;

                DirectionChoice = true;
                float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;

                // TODO: Add your game logic here.
                Angle += elapsed;
                float circle = MathHelper.Pi * 2;
                Angle = Angle % circle;
                //
            }
            return false;
        }

        protected float Angle;

        public override void Draw(GameTime gameTime)
        {
            if (Active)
            {
                spriteBatch.Draw(Arrow,
                                  new Rectangle((int)Position.X, (int)Position.Y-60, 20, 26),
                                  new Rectangle(0, 0, 41, 113), Color.White,
                                  3.14f, new Vector2(20, 113), SpriteEffects.None, 0);
            }

            spriteBatch.Draw(PlayerTex, Position - new Vector2(PlayerTex.Bounds.Width / 2, PlayerTex.Bounds.Height/ 2), Color.White);
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

                 if (Keyboard.GetState().IsKeyDown(Keys.A))
                     spriteBatch.Draw(Arrow,
                    new Rectangle((int)Position.X, (int)Position.Y, 41, 113),
                    new Rectangle(0, 0, 41, 113), Color.Red,
                    Angle+3.14f/2, new Vector2(20, 113), SpriteEffects.None, 0);

                 else if (Keyboard.GetState().IsKeyDown(Keys.D))
                 spriteBatch.Draw(Arrow,
                     new Rectangle((int)Position.X, (int)Position.Y, 41, 113),
                     new Rectangle(0, 0, 41, 113), Color.Red,
                     Angle - 3.14f / 2, new Vector2(20, 113), SpriteEffects.None, 0);

                 else if (Keyboard.GetState().IsKeyDown(Keys.W))
                 {
                     float angle = (float)Math.Sin(3*Angle) * 3.14f / 5;
                     spriteBatch.Draw(Arrow,
                         new Rectangle((int)Position.X, (int)Position.Y, 27, 75),
                         new Rectangle(0, 0, 41, 113), Color.Red,
                         Angle+angle, new Vector2(20, 113), SpriteEffects.None, 0);
                 }
            }
            base.Draw(gameTime);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Player p = obj as Player;
            if (p == null)
                return false;

            return p.Position == Position;
        }
    }
}
