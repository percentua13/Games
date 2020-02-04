using ArkanoidGame.Interfaces;
using System;
using System.Drawing;

namespace ArkanoidGame
{
    class Ball : IBall
    {
        #region properities
        public int m_MaxPlatformY { set; get; } =  Map.m_MapHeight / 4 + 2;
        public int m_iBallX { set; get; } = 0;
        public int m_iBallY { set; get; } = 0;
        public int m_iDirectionX { set; get; } = 1;
        public int m_iDirectionY { set; get; } = 1;
        public Image m_ArkanoidBall { set; get; }
        #endregion
        public Ball()
        {
            m_ArkanoidBall = new Bitmap(@"Pictures\ball_green.png");
        }

        public void SetInitialProperities(Set set)
        {
            #region
            Random PositionOfBall = new Random();
            m_iBallX = PositionOfBall.Next(0, Map.m_MapWidth - 1);
            m_iBallY = set.m_iSetY - 1;

            m_iDirectionX = 1;
            m_iDirectionY = -1;
            #endregion
        }

    }
}
