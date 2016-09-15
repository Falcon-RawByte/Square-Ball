using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using SquareBall.Ways;
using Microsoft.Xna.Framework.Input;

namespace SquareBall
{
    class EvilGoalkeeper :Enemy
    {
        Enemy e1, e2;

        public EvilGoalkeeper(Game game, SpriteBatch sb, Ball _ball, int x0, int y0, int R)
            : base(game, sb, _ball, x0 - R, y0, x0 - R, y0)
        {
            x0 = (int)(x0 * SBGame.Scale) + SBGame.LeftM;
            y0 = (int)(y0 * SBGame.Scale) + SBGame.TopM + 50;
            R = (int)(-R * SBGame.Scale);

            way = new CircleWay(x0, y0, -R);
        }

        public void Set(Enemy _e1, Enemy _e2)
        {
            e1 = _e1;
            e2 = _e2;
        }

        Counter predict = new Counter(9, 1);
        Vector2 prediction;

        public override void PassiveUpdate(GameTime gt)
        {
            if (Catching)
            {
                Vector2 t = Vector2.Zero;

                if (ball.isActive)
                {
                    if (predict.Add())
                    {
                        prediction = ball.Direction + ball.Position + ball.Tdir;
                        predict.Reset();
                    }

                    if (prediction.Y < Position.Y)
                        t = way.GetWayXYbyPointer((int)ball.Position.X, Position.Y - 2);
                    if (prediction.Y > Position.Y)
                        t = way.GetWayXYbyPointer((int)ball.Position.X, Position.Y + 2);
                        
                }

                if (t != Vector2.Zero)
                {
                    t = (t - Position);
                    Position += t*1.2f;
                }

            }
        }

        public override void Strike()
        {
            Random rnd = new Random();
            if (rnd.Next(10) > 5)
                Pass(e1);
            else Pass(e2);
        }

        public override void SwirlStrike()
        {
            Strike();
        }

    }
}
