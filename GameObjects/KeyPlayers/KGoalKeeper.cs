using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SquareBall.Ways;
using System.IO;

namespace SquareBall
{
    

    public class KGoalKeeper : GoalKeeper
    {

        Keys Up, Down, Strike;
        protected Texture2D PlayerTex2;
        public bool Orange = false;

        public void setKeys(Keys U, Keys D, Keys S)
        {
            Up = U;
            Down = D;
            Strike = S;
        }
 
        public KGoalKeeper(Game game, SpriteBatch sb, Ball _ball, int x0, int y0, int R)
            : base(game, sb, _ball, x0 , y0, R)
        {
            Up = Keys.W;
            Down = Keys.S;
            Strike = Keys.Space;

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
            if (Orange && PlayerTex != PlayerTex2)
                PlayerTex = PlayerTex2;

            Point xy = Mouse.GetState().Position;
            time += gt.ElapsedGameTime.Milliseconds;

                {
                    Vector2 t = Vector2.Zero;
                    if (Keyboard.GetState().IsKeyDown(Up))
                        velocity += -0.15f;
                    else if (Keyboard.GetState().IsKeyDown(Down))
                        velocity += 0.15f;

                    t = way.GetWayXYbyPointer((int)Position.X, (int)Position.Y + velocity);
                    velocity *= 0.98f;

                    if (t != Vector2.Zero)
                    {
                        t = (t - Position);
                            Position += t;
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

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            //if (Orange)
            //    spriteBatch.Draw(PlayerTex2, Position - new Vector2(PlayerTex.Bounds.Width / 2, PlayerTex.Bounds.Height / 2), Color.White);
        }
    }
}
