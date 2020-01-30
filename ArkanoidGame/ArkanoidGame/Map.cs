using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArkanoidGame
{
    class Map
    {
        public int iPlatformX { set; get; } = 0;
        public int iPlatformY { set; get; } = 0;

        Ball GameBall = null;
        const int mapWidth = 20;
        const int mapHeight = 30;
        const int PixelSize = 20;

        private int[,] map = null;

       public Map()
       {
            map = new int[mapHeight + 2, mapWidth + 2];

            GameBall = new Ball(@"Pictures\ball_green.png");
        }

        private void ClearGame()
        {
            for (int i = 0; i < mapHeight; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    map[i, j] = 0;
                }
            }
        }

        internal void SetInitProperities()
        {
            GameBall.MaxPlatformY = mapHeight / 4;

            iPlatformX = (mapWidth - 1) / 2;
            iPlatformY = mapHeight - 1;

            ClearGame();
            GeneratePlatforms();

            GameBall.SetInitProperities(this);

            map[iPlatformY, iPlatformX] = (int)Numbers.SET1;
            map[iPlatformY, iPlatformX + 1] = (int)Numbers.SET2;
            map[iPlatformY, iPlatformX + 2] = (int)Numbers.SET3;
            map[GameBall.iBallY, GameBall.iBallX] = (int)Numbers.BALL;
        }

        private void GeneratePlatforms()
        {
            for (int i = 0; i < GameBall?.MaxPlatformY; ++i)
            {
                for (int j = 0; j < mapWidth; j += 2)
                {
                    map[i, j] = (int)Numbers.BLOCK1;
                    map[i, j + 1] = (int)Numbers.BLOCK2;
                }
            }
        }

        internal bool AddLine(ref int Score)
        {
            ++Score;

            //New Game
            if (GameBall.MaxPlatformY + 1 >= mapHeight - 1) return false;


            for (int i = GameBall.MaxPlatformY; i > 0; --i)
            {
                for (int j = 0; j < mapWidth; j += 2)
                {
                    if (map[i - 1, j] != (int)Numbers.BALL && map[i, j] != (int)Numbers.BALL)
                        map[i, j] = map[i - 1, j];
                }
            }

            for (int j = 0; j < mapWidth; j += 2)
            {
                if (map[0, j] != (int)Numbers.BALL && map[0, j + 1] != (int)Numbers.BALL)
                {
                    map[0, j] = (int)Numbers.BLOCK1;
                    map[0, j + 1] = (int)Numbers.BLOCK2;
                }
            }

            ++GameBall.MaxPlatformY;

            return true;
        }

        public void DrawMap(Graphics g, Game game)
        {
            for (int i = 0; i < mapHeight; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    if (map[i, j] == (int)Numbers.SET1)
                    {
                        g.DrawImage(game.ArkanoidSet, new Rectangle(new Point(j * PixelSize, i * PixelSize), new Size(60, 20)), 0, 0, 64, 16, GraphicsUnit.Pixel);
                    }
                    switch (map[i, j])
                    {
                        case (int)Numbers.SET1:
                            g.DrawImage(game.ArkanoidSet, new Rectangle(new Point(j * PixelSize, i * PixelSize), new Size(60, 20)), 0, 0, 64, 16, GraphicsUnit.Pixel);
                            break;

                        case (int)Numbers.BALL:
                            g.DrawImage(GameBall.ArkanoidBall, new Rectangle(new Point(j * PixelSize, i * PixelSize), new Size(20, 20)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case (int)Numbers.BLOCK1:
                            g.DrawImage(game.ArkanoidPlatform_1, new Rectangle(new Point(j * PixelSize, i * PixelSize), new Size(40, 20)), 0, 0, 40, 20, GraphicsUnit.Pixel);
                            break;

                    }
                }
            }
            //!!!
            int a = 4;
          
        }

        internal bool IsCollide(ref int Score)
        {

            bool IsCollide = false;

            //WALL
            if (GameBall.iBallX + GameBall.iDirectionX >= mapWidth || GameBall.iBallX + GameBall.iDirectionX < 0)
            {
                GameBall.iDirectionX *= -1;
                IsCollide = true;
            }

            //WALL
            if (GameBall.iBallY + GameBall.iDirectionY >= mapHeight || GameBall.iBallY + GameBall.iDirectionY < 0)
            {
                GameBall.iDirectionY *= -1;
                IsCollide = true;
            }

            //SET
            if (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] >= (int)Numbers.SET1)
            {
                GameBall.iDirectionX *= -1;
                IsCollide = true;

                return IsCollide;
            }

            //SET
            if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] >= (int)Numbers.SET1)
            {
                GameBall.iDirectionY *= -1;
                IsCollide = true;

                return IsCollide;
            }
            //SET
            if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] >= (int)Numbers.SET1)
            {
                GameBall.iDirectionX *= -1;
                GameBall.iDirectionY *= -1;
                IsCollide = true;

                return IsCollide;
            }
            //BLOCK
            if (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] < (int)Numbers.SET1 && map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] != 0)
            {
                map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] = (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] == (int)Numbers.BALL) ? (int)Numbers.BALL : 0;

                if (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] < 10)
                {
                    map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX + 1] = (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX + 1] == (int)Numbers.BALL) ? (int)Numbers.BALL : 0;
                }
                else
                {
                    map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX - 1] = (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX - 1] == (int)Numbers.BALL) ? (int)Numbers.BALL : 0;
                }

                GameBall.iDirectionX *= -1;

                ++Score;
                IsCollide = true;

                // return IsCollide;
            }

            //BLOCK
            if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] < (int)Numbers.SET1 && map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] != 0)
            {
                map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] == (int)Numbers.BALL) ? (int)Numbers.BALL : 0;

                if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] < 10)
                {
                    map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + 1] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + 1] == (int)Numbers.BALL) ? (int)Numbers.BALL : 0;

                }
                else
                {
                    map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX - 1] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX - 1] == (int)Numbers.BALL) ? (int)Numbers.BALL : 0;
                }

                GameBall.iDirectionY *= -1;

                ++Score;

                IsCollide = true;

                //return IsCollide;
            }

            //BLOCK
            if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] < (int)Numbers.SET1 && map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] != 0)
            {

                map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] == (int)Numbers.BALL) ? (int)Numbers.BALL : 0;

                if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] < 10)
                {
                    map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX + 1] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX + 1] == (int)Numbers.BALL) ? (int)Numbers.BALL : 0;
                }
                else
                {
                    map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX - 1] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX - 1] == (int)Numbers.BALL) ? (int)Numbers.BALL : 0;
                }

                GameBall.iDirectionX *= -1;
                GameBall.iDirectionY *= -1;

                ++Score;
                IsCollide = true;

                return IsCollide;
            }


            return IsCollide;

        }

        internal void InputCheck(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right) return;

            map[iPlatformY, iPlatformX] = 0;
            map[iPlatformY, iPlatformX + 1] = 0;
            map[iPlatformY, iPlatformX + 2] = 0;

            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (iPlatformX + 3 < mapWidth)
                        ++iPlatformX;
                    break;

                case Keys.Left:
                    if (iPlatformX - 1 >= 0)
                        --iPlatformX;
                    break;
            }

            map[iPlatformY, iPlatformX] = (int)Numbers.SET1;
            map[iPlatformY, iPlatformX + 1] = (int)Numbers.SET2;
            map[iPlatformY, iPlatformX + 2] = (int)Numbers.SET3;
        }

        internal void update(object sender, EventArgs e, Game game)
        {
            //WALL
            if (GameBall.iBallY + GameBall.iDirectionY >= mapHeight)
            {
                game.Init();
            }

            if (!IsCollide(ref game.Score))
            {
                map[GameBall.iBallY, GameBall.iBallX] = 0;
                GameBall.iBallX += GameBall.iDirectionX;
                GameBall.iBallY += GameBall.iDirectionY;
                map[GameBall.iBallY, GameBall.iBallX] = (int)Numbers.BALL;

                if (game.Score % 15 == 0 && game.Score > 0)
                    if (!AddLine(ref game.Score)) game.Init();
            }


        }
        public int this[int i, int j]
        {
            get
            {
                return map[i, j];
            }
            set
            {
                map[i, j] = value;
            }
        }
    }
}
