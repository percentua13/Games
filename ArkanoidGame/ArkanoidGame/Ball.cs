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

        public Ball (string path)
        {
            ArkanoidBall = new Bitmap(path);
        }

        public void SetInitProperities(Map map)
        {
            
            Random PositionOfBall = new Random();
            iBallX = PositionOfBall.Next(0, Game.mapWidth - 1);// iPlatformX  - 3;
            iBallY = map.iPlatformY - 1;

            iDirectionX = 1;
            iDirectionY = -1;
        }



    }
}
