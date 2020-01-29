using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    public partial class Form1 : Form
    {
        enum Numbers
        {
            BALL = 8,
            SET1 = 9,
            SET2 = 99,
            SET3 = 999,
        }

        const int mapWidth = 20;
        const int mapHeight = 20;
        const int PixelSize = 20;

        private int[,] map = new int[mapHeight+2, mapWidth+2];
        private int iDirectionX = 0;
        private int iDirectionY = 0;

        private int iPlatformX = 0;
        private int iPlatformY = 0;

        private int iBallX = 0;
        private int iBallY = 0;

        private Image ArkanoidSet;
        private Image ArkanoidBall;
        public Form1()
        {
            InitializeComponent();
            timer1.Tick += new EventHandler(update);
            this.KeyUp += new KeyEventHandler(InputCheck);


            Init();
        }

        private void InputCheck(object sender, KeyEventArgs e)
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

        private void update(object sender, EventArgs e)
        {
            //WALL
            if (iBallY + iDirectionY >= mapHeight)
            {
                Init();
            }


            if (!IsCollide())
            {
                map[iBallY, iBallX] = 0;
                iBallX += iDirectionX;
                iBallY += iDirectionY;
                map[iBallY, iBallX] = (int)Numbers.BALL;
            }
           
            Invalidate();
        }

        private void ChangeCoordinatesWall()
        {
            if (iBallX + iDirectionX >= mapWidth || iBallX + iDirectionX < 0)
            {
                iDirectionX *= -1;
                return;
            }

            if (iBallY + iDirectionY >= mapHeight || iBallY + iDirectionY < 0)
            {
                iDirectionY *= -1;
                return;
            }
        }

        private  void ChangeCoordinatesSet()
        {
            try
            {
                if (map[iBallY + iDirectionY, iDirectionX] != 0)
                {
                    iDirectionY *= -1;
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private bool IsCollide()
        {
            
            bool IsCollide = false;

            //WALL
            if (iBallX + iDirectionX >= mapWidth || iBallX + iDirectionX < 0)
            {
                iDirectionX *= -1;
                IsCollide = true;
            }

            //WALL
            if (iBallY + iDirectionY >= mapHeight || iBallY + iDirectionY < 0)
            {
                iDirectionY *= -1;
                IsCollide = true;
            }
           
            //SET
            if (map[iBallY, iBallX + iDirectionX] != 0)
            {
                iDirectionX *= -1;
                IsCollide = true;

                return IsCollide;
            }

            //SET
            if (map[iBallY + iDirectionY, iBallX] != 0)
            {
                iDirectionY *= -1;
                IsCollide = true;

                return IsCollide;
            }
            //SET
            if (map[iBallY + iDirectionY, iBallX + iDirectionX] != 0)
            {
                iDirectionX *= -1;
                iDirectionY *= -1;
                IsCollide = true;
            }

            return IsCollide;
        
        }
        public void DrawArea(Graphics g)
        {
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, mapWidth * PixelSize, mapHeight * PixelSize));
        }
        public void DrawMap(Graphics g)
        { 
            for (int i = 0; i < mapHeight; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    if (map[i,j] == (int)Numbers.SET1)
                    {
                        g.DrawImage(ArkanoidSet, new Rectangle(new Point(j*PixelSize, i*PixelSize), new Size(60, 20)), 0, 0, 64, 16, GraphicsUnit.Pixel);
                    }
                    switch (map[i,j])
                    {
                        case (int)Numbers.SET1:
                            g.DrawImage(ArkanoidSet, new Rectangle(new Point(j * PixelSize, i * PixelSize), new Size(60, 20)), 0, 0, 64, 16, GraphicsUnit.Pixel);
                            break;
                        case (int)Numbers.BALL:
                            g.DrawImage(ArkanoidBall, new Rectangle(new Point(j * PixelSize, i * PixelSize), new Size(20, 20)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                    }
                }
            }
        }
        private void Init()
        {
            this.Width = mapWidth * PixelSize + 20;
            this.Height = mapHeight * PixelSize + 50;
            ArkanoidSet = new Bitmap(@"Pictures\set_violet.png");
            ArkanoidBall = new Bitmap(@"Pictures\ball_green.png");
            timer1.Interval = 50;

            iPlatformX = (mapWidth - 1) / 2;
            iPlatformY = mapHeight - 1;

            iBallX = iPlatformX + 1;
            iBallY = iPlatformY - 1;

            iDirectionX = 1;
            iDirectionY = -1;

            for (int i = 0; i < mapHeight; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    map[i, j] = 0;
                }
            }

            map[iPlatformY, iPlatformX] = (int)Numbers.SET1;
            map[iPlatformY, iPlatformX + 1] = (int)Numbers.SET2;
            map[iPlatformY, iPlatformX + 2] = (int)Numbers.SET3;
            map[iBallY, iBallX] = (int)Numbers.BALL;

            timer1.Start();
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            DrawMap(e.Graphics);
            DrawArea(e.Graphics);
        }
    }
}
