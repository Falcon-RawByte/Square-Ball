using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace SquareBall
{
    public class Ball : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected Texture2D BallTex;

        protected Rectangle Bounds;
        public Rectangle getBounds
        {
            get { return BallTex.Bounds; }
        }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
        }

        private Vector2 direction;
        public Vector2 Direction
        {
            get { return direction; }
        }

        private float velocity;
        public float Velocity
        {
            get { return velocity; }
        }

        protected float Angle;
        protected float Torque = 0;
        protected Vector2 tdir = Vector2.Zero;
        public Vector2 Tdir
        { get { return tdir; } }

        Vector2 RightGoalOrigin;
        int Goalradius;
        Vector2 LeftGoalOrigin;

        private Player sender;
        public Player Sender
        {
            get { return sender; }
        }


        protected bool Active;
        public bool isActive
        {
            get { return Active; }
        }
        public void Deactivate()
        {
            Active = false;
            position = new Vector2(600, 300);
        }

        Counter ThresholdX = new Counter(17, 1, true);
        Counter ThresholdY = new Counter(17, 1, true);

        public Ball(Game game, SpriteBatch sb, int width, int height)
            : base(game)
        {
            spriteBatch = sb;
            Bounds = new Rectangle(SBGame.LeftM, SBGame.TopM + 50, (int)(width * SBGame.Scale), (int)(height * SBGame.Scale));
            position = new Vector2(600, 300);
            using (Stream stream = TitleContainer.OpenStream("Content/Ball.png"))
            {
                BallTex = Texture2D.FromStream(game.GraphicsDevice, stream);
            }

        }

        public void SetGoalInfo(Point GR, Point GL, int R)
        {
            LeftGoalOrigin = new Vector2((float)(GL.X * SBGame.Scale) + SBGame.LeftM, (float)(GL.Y * SBGame.Scale) + SBGame.TopM + 50);
            RightGoalOrigin = new Vector2((float)(GR.X * SBGame.Scale) + SBGame.LeftM, (float)(GR.Y * SBGame.Scale) + SBGame.TopM + 50);
            Goalradius = (int)(R*SBGame.Scale);
        }

        public void Strike(Player _sender, float _velocity, Vector2 spawn, Vector2 _direction)
        {
            float a = Position.X-SBGame.ScreenW/2;
            SoundGenerator.Play(SBGame.Kick, true, (a)/Math.Abs(a));
            sender = _sender;
            velocity = _velocity;
            position = spawn;
            direction = _direction;
            Active = true;
            tdir = Vector2.Zero;
                Torque = 0;
        }

        public void Strike(Player sender, float velocity, Vector2 spawn, Vector2 direction, float torque)
        {
    
            Strike(sender, velocity, spawn, direction);
            Torque = torque;
            tdir = new Vector2(-direction.Y, direction.X);
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                Vector2 Dir = Direction*Velocity + tdir * Torque;
                position += Dir;
                if (ThresholdX.Add() && (-Bounds.Left + Position.X < 1 || -Position.X + Bounds.Right < 1 + BallTex.Bounds.Width))
                {
                    direction = Direction * Velocity + tdir * Torque;
                    direction.Normalize();
                    direction.X = -Direction.X;
                    position += new Vector2(Math.Sign(SBGame.ScreenW/2-Position.X), 0);
                    tdir = Vector2.Zero;
                    Torque = 0;
                   // Velocity -= Velocity * 0.1f;
                    SoundGenerator.Play(SBGame.Kick, true, 0);
                    ThresholdX.Reset();
                }

                if (ThresholdY.Add() && (Bounds.Bottom - Position.Y < 1 + BallTex.Bounds.Height || Position.Y - Bounds.Top < 1))
                {
                    direction += Direction * Velocity + tdir * Torque;
                    direction.Normalize();
                    direction.Y = -Direction.Y;
                    position += new Vector2(0, -Math.Sign(Position.Y - SBGame.ScreenH/2));
                    tdir = Vector2.Zero;
                    Torque = 0;
                  //  Velocity -= Velocity*0.1f;
                    SoundGenerator.Play(SBGame.Kick, true, 1);
                    ThresholdY.Reset();
                }
                velocity -= Velocity*Velocity/(7*500);
                Torque += Math.Sign(Torque) * Torque * Torque / (100);
                if (Torque > 6 || Torque < -6)
                    Torque = Math.Sign(Torque) * 6;
            }
            base.Update(gameTime);
        }

        public byte CheckGoal()
        {
            if (Position.X > RightGoalOrigin.X - Goalradius)
            {
                if ((RightGoalOrigin - (Position + new Vector2(getBounds.Width / 2, getBounds.Height / 2))).Length() < Goalradius)
                    return 1;
            }
            else if (Position.X < LeftGoalOrigin.X + Goalradius)
            {
                if ((LeftGoalOrigin - (Position + new Vector2(getBounds.Width / 2, getBounds.Height / 2))).Length() < Goalradius)
                    return 2;
            }
            return 0;
        }

        public void Stop()
        {
            velocity /= 1.1f;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Active)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Torque > 0)
                    Angle -= 4 * elapsed;
                if (Torque < 0)
                    Angle += 3 * elapsed;

                //spriteBatch.Draw(BallTex, Position, Color.White);
                spriteBatch.Draw(BallTex,
                    new Rectangle((int)Position.X + BallTex.Bounds.Width / 2, (int)Position.Y + BallTex.Bounds.Height/2,
                        BallTex.Bounds.Width, BallTex.Bounds.Height),
                    new Rectangle(0, 0, BallTex.Bounds.Width, BallTex.Bounds.Height),
                    Color.White,
                    Angle,
                    new Vector2(BallTex.Bounds.Width / 2, BallTex.Bounds.Height / 2),
                    SpriteEffects.None, 0);
            }
            base.Draw(gameTime);
        }
    }
}
