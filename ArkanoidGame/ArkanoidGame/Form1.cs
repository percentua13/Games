using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    public partial class Form1 : Form
    {
        const int mapWidth = 20;
        const int mapHeight = 20;
        const int PixelSize = 20;

        private int[,] map = new int[mapHeight, mapWidth];
        private int iDirectionX = 0;
        private int iDirectionY = 0;
        private int iPlatformX = 0;
        private int iPlatformY = 0;

        private Image ArkanoidSet;
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
            switch (e.KeyCode)
            {
                case Keys.Right:   
                    if (iPlatformX + 1 < mapWidth-1)  
                                ++iPlatformX;
                    break;

                case Keys.Left:
                    if (iPlatformX - 1 >= 0) 
                               --iPlatformX;
                    break;
            }

            map[iPlatformY, iPlatformX] = 9;
        }

        private void update(object sender, EventArgs e)
        {
            Invalidate();
        }

        public void DrawMap(Graphics g)
        { 
            for (int i = 0; i < mapHeight; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    if (map[i,j] == 9)
                    {
                        g.DrawImage(ArkanoidSet, new Rectangle(new Point(j*PixelSize, i*PixelSize), new Size(60, 20)), 0, 0, 64, 16, GraphicsUnit.Pixel);
                    }
                }
            }
        }
        private void Init()
        {
            this.Width = mapWidth * PixelSize + 35;
            this.Height = (mapHeight + 2) * PixelSize;
            ArkanoidSet = new Bitmap(@"Pictures\set_violet.png");
            timer1.Interval = 50;

            iPlatformX = (mapWidth - 1) / 2;
            iPlatformY = mapHeight - 1;

            for (int i = 0; i < mapHeight; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    map[i, j] = 0;
                }
            }

            map[iPlatformY, iPlatformX] = 9;

            timer1.Start();
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            DrawMap(e.Graphics);
        }
    }
}
