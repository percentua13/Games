using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    enum Numbers
    {
        BALL = 8,
        SET1 = 99,
        SET2 = 999,
        SET3 = 9999,
        BLOCK1 = 1,
        BLOCK2 = 11,
    }

    class Game
    {
        //private Ball GameBall;
        public const int mapWidth = 20;
        public const int mapHeight = 30;
        public const int PixelSize = 20;

        private Map map;

        public Image ArkanoidSet;
        public Image ArkanoidPlatform_1;
        public int Score  = 0;

        private /*readonly*/ Label lbl_Score;

        private Form form;

        private Timer TimerForGame;
        public Game(Form form, ref Timer timer)
        {
            lbl_Score = new Label() { Text = "Score : " + Score.ToString() };
            lbl_Score.Location = new Point(mapWidth * PixelSize + 5, (int)(mapHeight / 2.5 * PixelSize));

            form.Controls.Add(lbl_Score);

            this.form = form;
            
            TimerForGame = timer;

            TimerForGame.Interval = 60;

            
            map = new Map();

            Init();
        }
        internal void Init()
        {
            //INIT
            Score = 0;
           // GameBall.MaxPlatformY = mapHeight / 4;
            lbl_Score.Text = "Score : " + Score.ToString();

            //FORM
            form.Width = mapWidth * PixelSize + 100;
            form.Height = mapHeight * PixelSize + 48;


            ArkanoidSet = new Bitmap(@"Pictures\set_violet.png");
            
            ArkanoidPlatform_1 = new Bitmap(@"Pictures\platform_gold.png");

           
           // TimerForGame.Interval = 60;
           
            map?.SetInitProperities();

            //TimerForGame?.Start();
        }

        internal void OnPaint(object sender, PaintEventArgs e)
        {
            DrawMap(e.Graphics);
            DrawArea(e.Graphics);
        }

        public void DrawArea(Graphics g)
        {
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, mapWidth * PixelSize, mapHeight * PixelSize));
        }
        public void DrawMap(Graphics g)
        {
            map.DrawMap(g, this);
        }

        internal void update(object sender, EventArgs e)
        {
            map.update(sender, e, this);
             //????
            lbl_Score.Text = "Score : " + Score.ToString();
        }
        internal void InputCheck(object sender, KeyEventArgs e)
        {
            map.InputCheck(sender, e);
        }
    }
}
