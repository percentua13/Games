using System.Drawing;

namespace ArkanoidGame
{
    class Set
    {
        #region fields
        public Image SetImage;
        public int m_iSetX { set; get; } = 0;
        public int m_iSetY { set; get; } = 0;
        #endregion
        public Set()
        {
            #region
            SetImage = new Bitmap(@"Pictures\set_gold.png");
            #endregion
        }
    }
}
