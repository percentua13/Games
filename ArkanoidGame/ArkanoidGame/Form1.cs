using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    public partial class ArkanoidForm : Form
    {
        int seconds = 0,
            minutes = 0,
            hours = 0;

        Game ArkanoidGame = null;
        Label lbl_Time;
        public ArkanoidForm()
        {
            
            InitializeComponent();

            TimerForGame = new Timer() { Interval = 60 };
            Clock = new Timer() { Interval = 1000 };

            ArkanoidGame = new Game(this, ref TimerForGame);


            lbl_Time = new Label();
            lbl_Time.Font = new Font("MV Boli", 10);
            lbl_Time.Location = new Point(4, this.Height-70);
            lbl_Time.AutoSize = true;
            this.Controls.Add(lbl_Time);

            TimerForGame.Tick += new EventHandler(Tick);

            TimerForGame.Tick += new EventHandler(update);
            KeyUp += new KeyEventHandler(InputCheck);
            KeyDown += InputCheck;


            TimerForGame.Start();
            Clock.Start();
            
            Invalidate();
        }

        internal void update(object sender, EventArgs e)
        {
            ArkanoidGame.update(sender, e);
            Invalidate();
        }
        internal void Tick(object sender, EventArgs e)
        {
            ++seconds;
            minutes += seconds / 60;
            hours += minutes / 60;
            minutes %= 60;
            seconds %= 60;
            
            lbl_Time.Text = $"Time : {hours}:{minutes}:{seconds}";
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
