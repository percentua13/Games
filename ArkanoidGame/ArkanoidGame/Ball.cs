using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkanoidGame
{
    class Ball
    {
        public int MaxPlatformY { set; get; } = /*mapHeight/4*/ 25/4;
        public int iBallX { set; get; } = 0;
        public int iBallY { set; get; } = 0;

        public int iDirectionX { set; get; } = 1;
        public int iDirectionY { set; get; } = 1;

        public Image ArkanoidBall { set; get; }

        public Ball ()
        {
            ArkanoidBall = new Bitmap(@"Pictures\ball_green.png");
        }

        public void SetInitProperities(Set set)
        {
            
            Random PositionOfBall = new Random();
            iBallX = PositionOfBall.Next(0, Map.mapWidth - 1);// iPlatformX  - 3;
            iBallY = set.iSetY - 1;

            iDirectionX = 1;
            iDirectionY = -1;
        }



    }
}
