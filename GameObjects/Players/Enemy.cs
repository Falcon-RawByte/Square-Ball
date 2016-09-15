using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace SquareBall
{
    public class Enemy : Player
    {

        public Enemy(Game game, SpriteBatch sb, Ball _ball, int Sx, int Sy, int Fx, int Fy)
            : base(game, sb, _ball, Sx, Sy, Fx, Fy)
        {
            TexName = "Content/Enemy.png";
            using (Stream stream = TitleContainer.OpenStream(TexName))
            {
                PlayerTex = Texture2D.FromStream(game.GraphicsDevice, stream);
            }
        }

        public virtual void PassiveUpdate(GameTime gt)
        {
            if (Catching)
            {
                Vector2 t = way.GetWayXYbyPointer((int)ball.Position.X, (int)ball.Position.Y);
                if (t != Vector2.Zero)
                {
                    t = (t - Position);
                    if (t.LengthSquared() > 0.1)
                    {
                        t.Normalize();
                        Position += t * 3;
                    }
                }
                    
            }

            //if (HasBall)
            //{

            //    // So... What can we do? First of all, we can give a pass.
            //    // We can strike forward
            //    // We Can strike with angle
            //    // We can SwirlyStrike
                
            //    float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;

                

            //    Angle += 3*elapsed;
            //    float circle = MathHelper.Pi * 2;
            //    Angle = Angle % circle;
            //    if (phantomBall(Angle))
            //    {
            //       ball.Strike(this, 7, Position, new Vector2((float)Math.Sin(Angle), -(float)Math.Cos(Angle)));
            //       //ball.Strike(this, 7, new Vector2(1000, (float)((1000 - Position.X) - 2 * Position.Y) / (float)Math.Tan(Angle)), new Vector2((float)Math.Sin(Angle), -(float)Math.Cos(Angle)));
            //        HasBall = false;
            //    }
            //}
        }

        //bool phantomBall(float a)
        //{
        //    double Y = ((1000 - Position.X) - 2 * Position.Y) / Math.Tan(a);
        //    return (Y > 300 && Y < 600);
        //}

        private double PointToLine(Vector2 p, Vector2 st, Vector2 f)
        {
            Vector2 AB = (p - st);
            Vector2 AC = (p - f);

            float ABAC = AB.X * AC.X + AB.Y * AC.Y;
            float cos = (ABAC / (AB.Length() * AC.Length()));

            return AB.Length() * Math.Sqrt(1 - cos*cos);
        }

        public double CalculateStrike(PlayerController pc)
        {
            Random rnd = new Random();

            int TX = (int)(1000 * SBGame.Scale + SBGame.LeftM);
            int TY = (int)(300 * SBGame.Scale + SBGame.TopM + 50 +rnd.Next(300) - 150);

            Angle = (float)Math.Atan((-TY + Position.Y)/(-TX + Position.X));
            double time = (TX - Position.X) / (6 * Math.Cos(Angle));

            foreach (Player p in pc.Players)
            {
                double close = PointToLine(p.Position, Position, new Vector2(TX, TY))/10;
                GoalKeeper gk = p as GoalKeeper;
                if (gk != null)
                    time += 3 * close;
                else
                    time += 1.5*close;
            }

            return time;
        }

        private float SAngle;

        public double CalculateSwirlStrike(PlayerController pc)
        {
            

            Random rnd = new Random();

            int TX = (int)(1000 * SBGame.Scale + SBGame.LeftM);
            int TY = (int)(300 * SBGame.Scale + SBGame.TopM + 50);// +rnd.Next(300) - 150;

            if (Position.Y - SBGame.ScreenH / 2 > 0)
            {
                TY -= 250;
            }
            else
            {
                TY += 250;
            }


            SAngle = (float)Math.Atan((-TY + Position.Y) / (-TX + Position.X));
            double time = 1.3*(TX - Position.X) / (6 * Math.Cos(Angle));

            foreach (Player p in pc.Players)
            {
                double close = PointToLine(p.Position, Position, new Vector2(TX, TY))/10;
                GoalKeeper gk = p as GoalKeeper;
                if (gk != null)
                    time += close;
                else
                    time += 2.5*close;
            }

            return time;
        }

        //public override void Draw(GameTime gameTime)
        //{
        //    spriteBatch.Draw(Arrow,
        //            new Rectangle((int)Position.X, (int)Position.Y, 41, 113),
        //            new Rectangle(0, 0, 41, 113), Color.White,
        //            Angle + 3.14f / 2, new Vector2(20, 113), SpriteEffects.None, 0);

        //    spriteBatch.Draw(Arrow,
        //            new Rectangle((int)Position.X, (int)Position.Y, 41, 113),
        //            new Rectangle(0, 0, 41, 113), Color.Red,
        //            SAngle + 3.14f / 2, new Vector2(20, 113), SpriteEffects.None, 0);
        //    base.Draw(gameTime);
        //}

        public virtual void Strike()
        {
            ball.Strike(this, 10, Position, new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle)));
            HasBall = false;
        }

        public virtual void SwirlStrike()
        {
            Random rnd = new Random();
            float t;

            //if (rnd.Next(10) > 5)
            //    t = (float)rnd.NextDouble() + 0.8f;
            //else
            //    t = -(float)rnd.NextDouble() - 0.8f;

            if (Position.Y - SBGame.ScreenH / 2 > 0)
                t = (float)rnd.NextDouble() + 0.8f;
            
            else
                t = -(float)rnd.NextDouble() - 0.8f;


            ball.Strike(this, 9, Position, new Vector2((float)Math.Cos(Angle), (float)Math.Sin(SAngle)), t);
            HasBall = false;
        }


        public void Pass(Enemy en)
        {
            Random rnd = new Random();
            double angle = System.Math.Atan((rnd.Next(10)+en.Position.X - Position.X) / (en.Position.Y - Position.Y));
            if (Position.Y < en.Position.Y)
                ball.Strike(this, 6, Position, new Vector2((float)System.Math.Sin(angle), (float)System.Math.Cos(angle)));
            else
                ball.Strike(this, 6, Position, new Vector2(-(float)System.Math.Sin(angle), -(float)System.Math.Cos(angle)));
            HasBall = false;
            en.Catching = true;
        }

        public bool Catching = false;
    }
}
