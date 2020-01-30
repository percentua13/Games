using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    class Map
    {
        int KoefToAddLine = 1;
        Ball GameBall = null;
        internal const int mapWidth = 20;
        internal const int mapHeight = 30;
        internal const int PixelSize = 20;

        private int[,] map = null;

        private Platform ArkanoidPlatform;

        private Set ArkanoidSet;

       public Map()
       {
            map = new int[mapHeight + 2, mapWidth + 2];

            GameBall = new Ball();

            ArkanoidSet = new Set();

            ArkanoidPlatform = new Platform();
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

            ArkanoidSet.iSetX = (mapWidth-3) / 2;
            ArkanoidSet.iSetY = mapHeight - 1;

            ClearGame();
            GeneratePlatforms();

            GameBall.SetInitProperities(ArkanoidSet);

            map[ArkanoidSet.iSetY, ArkanoidSet.iSetX] = (int)DefinedNumbersForGame.SET1;
            map[ArkanoidSet.iSetY, ArkanoidSet.iSetX + 1] = (int)DefinedNumbersForGame.SET2;
            map[ArkanoidSet.iSetY, ArkanoidSet.iSetX + 2] = (int)DefinedNumbersForGame.SET3;
            map[GameBall.iBallY, GameBall.iBallX] = (int)DefinedNumbersForGame.BALL;

        }

        private void GeneratePlatforms()
        {
            Random rand = new Random();

            for (int i = 0; i < GameBall?.MaxPlatformY; ++i)
            {
                for (int j = 0; j < mapWidth; j += 2)
                {
                    
                    int index = rand.Next(1, 9);
                    map[i, j] = index;
                    map[i, j + 1] = index * 10;
                }
            }
        }

        internal bool AddLine(ref int Score)
        {
            //New Game
            if (GameBall.MaxPlatformY + 1 >= mapHeight - 1) return false;


            for (int i = GameBall.MaxPlatformY; i > 0; --i)
            {
                for (int j = 0; j < mapWidth; j += 2)
                {
                    if (map[i - 1, j] != (int)DefinedNumbersForGame.BALL && map[i, j] != (int)DefinedNumbersForGame.BALL)
                        map[i, j] = map[i - 1, j];
                }
            }

            Random rand = new Random();
            for (int j = 0; j < mapWidth; j += 2)
            {
                if (map[0, j] != (int)DefinedNumbersForGame.BALL && map[0, j + 1] != (int)DefinedNumbersForGame.BALL)
                {
                    map[0, j] = rand.Next(1, Platform.CountOfPlatforms);
                    map[0, j + 1] = map[0, j]*10;
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
                    if (map[i, j] == (int)DefinedNumbersForGame.SET1)
                    {
                        g.DrawImage(ArkanoidSet.SetImage, new Rectangle(new Point((int)DefinedNumbersForGame.LEFT_MARGIN + j * PixelSize, i * PixelSize), new Size(60, 20)), 0, 0, 64, 16, GraphicsUnit.Pixel);
                    }

                    switch (map[i, j])
                    {
                        case (int)DefinedNumbersForGame.SET1:
                            g.DrawImage(ArkanoidSet.SetImage, new Rectangle(new Point((int)DefinedNumbersForGame.LEFT_MARGIN + j * PixelSize, i * PixelSize), new Size(60, 20)), 0, 0, 64, 16, GraphicsUnit.Pixel);
                            break;

                        case (int)DefinedNumbersForGame.BALL:
                            g.DrawImage(GameBall.ArkanoidBall, new Rectangle(new Point((int)DefinedNumbersForGame.LEFT_MARGIN + j * PixelSize, i * PixelSize), new Size(20, 20)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 0:
                            break;

                        default:
                            int index = map[i, j]-1;

                            if (index < Platform.CountOfPlatforms) 
                                 g.DrawImage(ArkanoidPlatform.PlatformImage, new Rectangle(new Point((int)DefinedNumbersForGame.LEFT_MARGIN + j * PixelSize, i * PixelSize), new Size(40, 20)), ArkanoidPlatform[index].X_1, ArkanoidPlatform[index].Y_1, ArkanoidPlatform[index].X_Size, ArkanoidPlatform[index].Y_Size, GraphicsUnit.Pixel);
                           
                            break;

                    }
                }
            }       
        }

        internal bool IsCollide(ref int Score)
        {

            bool IsCollide = false;

            //WALL
            if (GameBall.iBallX + GameBall.iDirectionX  > mapWidth - 1 || GameBall.iBallX + GameBall.iDirectionX  < 0)
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
            if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] >= (int)DefinedNumbersForGame.SET1)
            {
                GameBall.iDirectionY *= -1;
                IsCollide = true;

                return IsCollide;
            }

            //SET
            if (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] >= (int)DefinedNumbersForGame.SET1)
            {
                GameBall.iDirectionX *= -1;
                IsCollide = true;

                return IsCollide;
            }

            //SET
            if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] >= (int)DefinedNumbersForGame.SET1)
            {
                GameBall.iDirectionX *= -1;
                GameBall.iDirectionY *= -1;
                IsCollide = true;

               return IsCollide;
            }
            //BLOCK
            if (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] < (int)DefinedNumbersForGame.SET1 && map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] != 0)
            {
                map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] = (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] == (int)DefinedNumbersForGame.BALL) ? (int)DefinedNumbersForGame.BALL : 0;

                if (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX] < 10)
                {
                    map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX + 1] = (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX + 1] == (int)DefinedNumbersForGame.BALL) ? (int)DefinedNumbersForGame.BALL : 0;
                }
                else
                {
                    map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX - 1] = (map[GameBall.iBallY, GameBall.iBallX + GameBall.iDirectionX - 1] == (int)DefinedNumbersForGame.BALL) ? (int)DefinedNumbersForGame.BALL : 0;
                }

                GameBall.iDirectionX *= -1;

                ++Score;

                IsCollide = true;

                return IsCollide;
            }

            //BLOCK
            if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] < (int)DefinedNumbersForGame.SET1 && map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] != 0)
            {
                map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] == (int)DefinedNumbersForGame.BALL) ? (int)DefinedNumbersForGame.BALL : 0;

                if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX] < 10)
                {
                    map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + 1] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + 1] == (int)DefinedNumbersForGame.BALL) ? (int)DefinedNumbersForGame.BALL : 0;

                }
                else
                {
                    map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX - 1] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX - 1] == (int)DefinedNumbersForGame.BALL) ? (int)DefinedNumbersForGame.BALL : 0;
                }

                GameBall.iDirectionY *= -1;

                ++Score;

                IsCollide = true;

                return IsCollide;
            }

            //BLOCK
            if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] < (int)DefinedNumbersForGame.SET1 && map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] != 0)
            {

                map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] == (int)DefinedNumbersForGame.BALL) ? (int)DefinedNumbersForGame.BALL : 0;

                if (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX] < 10)
                {
                    map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX + 1] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX + 1] == (int)DefinedNumbersForGame.BALL) ? (int)DefinedNumbersForGame.BALL : 0;
                }
                else
                {
                    map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX - 1] = (map[GameBall.iBallY + GameBall.iDirectionY, GameBall.iBallX + GameBall.iDirectionX - 1] == (int)DefinedNumbersForGame.BALL) ? (int)DefinedNumbersForGame.BALL : 0;
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

            //Set has length = 3 points
            map[ArkanoidSet.iSetY, ArkanoidSet.iSetX] = 0;
            map[ArkanoidSet.iSetY, ArkanoidSet.iSetX + 1] = 0;
            map[ArkanoidSet.iSetY, ArkanoidSet.iSetX + 2] = 0;

            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (ArkanoidSet.iSetX + 3 < mapWidth)
                        ++ArkanoidSet.iSetX;
                    break;

                case Keys.Left:
                    if (ArkanoidSet.iSetX - 1 >= 0)
                        --ArkanoidSet.iSetX;
                    break;
            }

            map[ArkanoidSet.iSetY, ArkanoidSet.iSetX] = (int)DefinedNumbersForGame.SET1;
            map[ArkanoidSet.iSetY, ArkanoidSet.iSetX + 1] = (int)DefinedNumbersForGame.SET2;
            map[ArkanoidSet.iSetY, ArkanoidSet.iSetX + 2] = (int)DefinedNumbersForGame.SET3;
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
                map[GameBall.iBallY, GameBall.iBallX] = (int)DefinedNumbersForGame.BALL;

                if (KoefToAddLine * 15 <= game.Score)
                {
                    if (!AddLine(ref game.Score)) game.Init();
                    ++KoefToAddLine;
                }
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
