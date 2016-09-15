using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

namespace SquareBall
{
    
    public class PlayerController
    {
        public long LastStrike
        {
            get
            {
                long max = Players[0].LastStrike;
                if (Players[1].LastStrike > max)
                    return Players[1].LastStrike;
                return max;
            }
        }

        protected bool GlobalTurn = false;

        protected Counter Threshold = new Counter(15, 1);

        public List<Player> Players;

        public PlayerController()
        {
            Players = new List<Player>();
        }

        public virtual void Add(Player pl)
        {
            GoalKeeper gk = pl as GoalKeeper;
            if (gk != null)
                GK = gk;
            Players.Add(pl);
        }

        protected GoalKeeper GK;

        public virtual void Update(GameTime gt, Ball ball)
        {
            foreach (Player player in Players)
            {
                if (ball.isActive)
                {
                    Rectangle b1 = ball.getBounds;
                    Rectangle b = new Rectangle(-b1.Left + (int)ball.Position.X, (int)ball.Position.Y - b1.Top, b1.Width, b1.Height);
                    Rectangle p1 = player.getBounds;
                    Rectangle p = new Rectangle(-p1.Left + (int)player.Position.X, (int)player.Position.Y - p1.Top, p1.Width, p1.Height);
                    if (b.Intersects(p))
                    {
                        if (ball.isActive)
                        {
                            if (ball.Sender != player || Threshold.Add())
                            {
                                ball.Deactivate();
                                player.GetBall();
                                Threshold.Reset();
                            }
                        }
                    }
                }

                if (player.ActiveUpdate(gt))
                {
                    //Может не понадобиться

                    foreach (Player _player in Players)
                    {

                        if (_player != player)
                        {
                            if (player == GK)
                            {
                                Point xy = Mouse.GetState().Position;
                                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                    if (_player.CheckHover(xy))
                                        GK.Pass(_player);
                            }
                            _player.Deactivate();
                        }
                    }
                }
            }

        }

        public void Draw(GameTime gt)
        {
            foreach (Player player in Players)
                player.Draw(gt);
        }

        public virtual void GiveBall()
        {
            GK.GetBall();
        }
    }
}




