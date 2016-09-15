    using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SquareBall.Ways;

namespace SquareBall
{
    

    public class GoalKeeper : Player
    {
 
        public GoalKeeper(Game game, SpriteBatch sb, Ball _ball, int x0, int y0, int R)
            : base(game, sb, _ball, x0 - R, y0, x0 - R, y0)
        {
            x0 = (int)(x0 * SBGame.Scale) + SBGame.LeftM;
            y0 = (int)(y0 * SBGame.Scale) + SBGame.TopM + 50;
            R = (int)(R*SBGame.Scale);

            way = new CircleWay(x0, y0, R);
        }

        public override bool ActiveUpdate(GameTime gt)
        {
            Point xy = Mouse.GetState().Position;
            time += gt.ElapsedGameTime.Milliseconds;

                {
                    Vector2 t = Vector2.Zero;
                    if (Keyboard.GetState().IsKeyDown(Keys.Q))
                        t = way.GetWayXYbyPointer((int)Position.X, (int)Position.Y - 3);
                    else if (Keyboard.GetState().IsKeyDown(Keys.E))
                        t = way.GetWayXYbyPointer((int)Position.X, (int)Position.Y + 3);

                    if (t != Vector2.Zero)
                    {
                        t = (t - Position);
                            Position += t;
                    }
                }
            

            if (time > 1000 && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (
           (xy.X >= Position.X - PlayerTex.Bounds.Width / 2
            && xy.X <= Position.X + PlayerTex.Bounds.Width / 2)
            && (xy.Y >= Position.Y - PlayerTex.Bounds.Height / 2
            && xy.Y <= Position.Y + PlayerTex.Bounds.Height / 2))
                {
                    Active = !Active;
                    time = 0;
                    return true;
                }
            }



            return HasBall;
        }

        public void Pass(Player player)
        {
            
            double angle = System.Math.Atan((player.Position.X - Position.X)/(player.Position.Y - Position.Y));
            if (Position.Y < player.Position.Y)
                ball.Strike(this, 6, Position, new Vector2((float)System.Math.Sin(angle), (float)System.Math.Cos(angle)));
            else
                ball.Strike(this, 6, Position, new Vector2(-(float)System.Math.Sin(angle), -(float)System.Math.Cos(angle)));
            HasBall = false;
        }
    }
}
