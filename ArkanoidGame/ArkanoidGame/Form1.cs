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
            SET1 = 99,
            SET2 = 999,
            SET3 = 9999,
            BLOCK1 = 1,
            BLOCK2 = 11,
        }

        //const int mapWidth = 20;
        //const int mapHeight = 30;
        //const int PixelSize = 20;

        //private int[,] map = new int[mapHeight+2, mapWidth+2];
        //  private int iDirectionX = 0;
        //private int iDirectionY = 0;

        //private int iPlatformX = 0;
        //private int iPlatformY = 0;

        //private int MaxPlatformY = mapHeight / 4;
        // private int iBallX = 0;
        // private int iBallY = 0;
        // private Image ArkanoidBall;

        // private Image ArkanoidSet;
        // private Image ArkanoidPlatform_1;


        //private int Score = 0;
        Game ArkanoidGame = null;

        public Form1()
        {
            InitializeComponent();

            TimerForGame = new Timer();

            ArkanoidGame = new Game(this, ref TimerForGame);

            TimerForGame.Tick += new EventHandler(update);
        

            KeyUp += new KeyEventHandler(InputCheck);
            KeyDown += InputCheck;

            //TimerForGame.Interval = 60;

            TimerForGame.Start();
            
            Invalidate();

        }

        internal void update(object sender, EventArgs e)
        {
            ArkanoidGame.update(sender, e);
            Invalidate();
        }
        internal void InputCheck(object sender, KeyEventArgs e)
        {
            ArkanoidGame.InputCheck(sender, e);
            Invalidate();
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            ArkanoidGame?.OnPaint(sender, e);
        }

       


    }
}
