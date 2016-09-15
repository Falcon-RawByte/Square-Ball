using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

namespace SquareBall
{
    
    public class PlayerIIController : PlayerController
    {

        private int n;

        Counter delay = new Counter(17, 1);

        

        public override void Add(Player pl)
        {
            KGoalKeeper gk = pl as KGoalKeeper;
            if (gk != null)
            {
                GK = gk;
                //gk.setKeys(Keys.O, Keys.L, Keys.Enter);
                gk.Orange = true;
            }

            KPlayer kp = pl as KPlayer;
            if (kp != null)
            {
                //kp.setKeys(Keys.K, Keys.OemSemicolon, Keys.O, Keys.L, Keys.Enter);
                kp.Orange = true;
            }
            
            Players.Add(pl);
        }

        public override void Update(GameTime gt, Ball ball)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Q) & delay.Add())
            {
                delay.Reset();
                Players[0].Activate();
                n = 0;
                    foreach (Player _player in Players)
                        if (_player != Players[0])
                        {
                            _player.Deactivate();
                        } 
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E) & delay.Add())
            {
                delay.Reset();
                Players[1].Activate();
                n = 1;
                foreach (Player _player in Players)
                    if (_player != Players[1])
                    {
                        _player.Deactivate();
                    }
            }


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
                    foreach (Player _player in Players)
                        if (player == GK)
                            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                            GK.Pass(Players[n]);
            }
        }

    }
}




