using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SquareBall
{
    class EnemyController : PlayerController
    {
        bool Turn = false;
        Counter think = new Counter(100, 1);

        EvilGoalkeeper EG;

        public override void Add(Player pl)
        {
            base.Add(pl);
            EvilGoalkeeper eg = pl as EvilGoalkeeper;
            if (eg != null)
            {
                EG = eg;
                EG.Catching = true;
                EG.Set((Enemy)Players[0], (Enemy)Players[1]);
            }
        }

        private PlayerController PC;

        public EnemyController(PlayerController pc) : base() 
        {
            PC = pc;
        }

        Enemy curEnemy;
        Enemy mEnemy;
        double minT = 5000;
        byte StrikeType = 0;

        public int DebugInfo_Curl = 0;
        public int DebugInfo_Str = 0;
        public void DrawDebugInfo(SpriteBatch spriteBatch)
        {
           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt, Ball ball)
        {
            if (curEnemy == null)
            {
                curEnemy = (Enemy)Players[1];
                mEnemy = (Enemy)Players[1];
            }
            foreach (Enemy enemy in Players)
            {
                if (ball.isActive)
                {
                    Rectangle b1 = ball.getBounds;
                    Rectangle b = new Rectangle(-b1.Left + (int)ball.Position.X, (int)ball.Position.Y - b1.Top, b1.Width, b1.Height);
                    Rectangle p1 = enemy.getBounds;
                    Rectangle p = new Rectangle(-p1.Left + (int)enemy.Position.X, (int)enemy.Position.Y - p1.Top, p1.Width, p1.Height);
                    if (b.Intersects(p))
                    {
                        if (ball.isActive)
                        {

                            if (ball.Sender != enemy || Threshold.Add())
                            {
                                ball.Stop();
                                ball.Deactivate();
                                enemy.GetBall();
                                curEnemy = enemy;
                                enemy.Catching = false;
                                Turn = true;
                                GlobalTurn = true;
                                Threshold.Reset();
                            }
                        }
                    }
                }
                enemy.PassiveUpdate(gt);
            }

            if (GlobalTurn)
            {

                if (Turn)
                {
                    if (minT == 5000)
                    foreach (Enemy enemy in Players)
                    {

                            double S = enemy.CalculateStrike(PC);
                            double C = enemy.CalculateSwirlStrike(PC);
                            byte StrikeT = 0;
                            double T = 0;
                            if (S < C)
                            {
                                T = S;
                                StrikeT = 0;
                                DebugInfo_Str++;
                            }
                            else
                            {
                                T = C;
                                StrikeT = 1;
                                DebugInfo_Curl++;
                            }

                            if (enemy.GetActive)
                                curEnemy = enemy;

                            if (minT == 0)
                            {
                                minT = T;
                                mEnemy = enemy;
                                StrikeType = StrikeT;
                            }
                            else if (T < minT)
                            {
                                minT = T;
                                mEnemy = enemy;
                                StrikeType = StrikeT;
                            }
                    }

                    Random rnd = new Random();
                    
                    if (curEnemy != mEnemy)
                    {
                        if (think.Add())
                        {
                            curEnemy.Pass(mEnemy);
                            Turn = false;
                            think.Reset(60 + rnd.Next(60)-10);
                        }
                    }

                    if (curEnemy == mEnemy && curEnemy.GetActive)
                    {
                        if (think.Add())
                        {
                            if (StrikeType == 0)
                                curEnemy.Strike();
                            else if (StrikeType == 1)
                                curEnemy.SwirlStrike();
                            Turn = false;
                            GlobalTurn = false;
                            think.Reset();
                            minT = 5000;
                        }
                    }

                }
            }
            else
            {
                foreach (Enemy en in Players)
                {
                    if (ball.isActive && (en.Position - ball.Position).Length() > 1 &&
                        (ball.Position.Y - 300 - SBGame.TopM) * (en.Position.Y - 300 - SBGame.TopM) > 0)
                    {
                        en.Catching = true;
                    }
                    else en.Catching = false;
                    EG.Catching = true;
                }
            }
        }

        public override void GiveBall()
        {
            GlobalTurn = true;
            Turn = true;
            EG.GetBall();
        }
    }
}
