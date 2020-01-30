using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkanoidGame
{
    class Set
    {
        public Image SetImage;

        public int iSetX { set; get; } = 0;
        public int iSetY { set; get; } = 0;
        public Set()
        {
            SetImage = new Bitmap(@"Pictures\set_gold.png");
        }
    }
}
