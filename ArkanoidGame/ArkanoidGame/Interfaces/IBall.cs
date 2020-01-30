using System.Drawing;

namespace ArkanoidGame.Interfaces
{
    interface IBall
    {
        #region properities
        public int m_MaxPlatformY { set; get; }
        public int m_iBallX { set; get; }
        public int m_iBallY { set; get; }

        public int m_iDirectionX { set; get; } 
        public int m_iDirectionY { set; get; } 

        public Image m_ArkanoidBall { set; get; }
        #endregion

        public void SetInitProperities(Set set);
    }
}
