using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    public partial class Form1 : Form
    {
        

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
