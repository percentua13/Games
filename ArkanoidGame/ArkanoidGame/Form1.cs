using ArkanoidGame.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    public partial class ArkanoidForm : Form
    {
        public ArkanoidForm()
        {
            #region
            InitializeComponent();

            GameInitialization();
            #endregion
        }


        static class PassedTime
        {
            #region
            static public int seconds {get; set;} = 0;
            static public int minutes { get; set; } = 0;
            static public int hours { get; set; } = 0;
            #endregion
        }

        #region fields
        IGame ArkanoidGame;
        Label lbl_Time;
        Label lbl_Score;
        #endregion

        private void CreateLabel(out Label label)
        {
            #region
            label = new Label();
            label.Font = new Font("MV Boli", 10);
            label.AutoSize = true;
            this.Controls.Add(label);
            #endregion
        }

        private void GameInitialization()
        {
            #region
            //Create form elements
            CreateLabel(out lbl_Score);
            lbl_Score.Location = new Point(0, (int)(Map.m_MapHeight / 2.5 * (int)ENUM_DefinedNumbersForGame.PIXEL_SIZE));

            CreateLabel(out lbl_Time);
            lbl_Time.Location = new Point(4, this.Height - 70);

            TimerForGame = new Timer() { Interval = 60 };
            Clock = new Timer() { Interval = 1000 };


            //Create new game and set form size properities
            int FormWidth, FormHeight;

            ArkanoidGame = new Game(ref lbl_Score, out FormWidth, out FormHeight);

            this.Height = FormHeight;
            this.Width = FormWidth;

            //Add events
            TimerForGame.Tick += new EventHandler(Tick);
            TimerForGame.Tick += new EventHandler(UpdateGameInfo_Win);

            KeyUp += new KeyEventHandler(KeyInputCheck);
            KeyDown += KeyInputCheck;


            //Activate timers
            TimerForGame.Start();
            Clock.Start();
            #endregion
        }
       

        internal void UpdateGameInfo_Win(object sender, EventArgs e)
        {
            if (ArkanoidGame.UpdateGameInfo(sender, e))
            {
                TimerForGame.Tick -= UpdateGameInfo_Win;

                MessageBox.Show("You're win ! c:");

                TimerForGame.Tick += new EventHandler(UpdateGameInfo_Win);
            }
            Invalidate();
        }

        //Find how much time is passed
        internal void Tick(object sender, EventArgs e)
        {
            #region
            ++PassedTime.seconds;

            PassedTime.minutes += PassedTime.seconds / 60;
            PassedTime.hours += PassedTime.minutes / 60;
            PassedTime.minutes %= 60;
            PassedTime.seconds %= 60;
            
            lbl_Time.Text = $"Time : {PassedTime.hours}:{PassedTime.minutes}:{PassedTime.seconds}";
            #endregion
        }
        internal void KeyInputCheck(object sender, KeyEventArgs e)
        {
            ArkanoidGame?.KeyInputCheck(sender, e);
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            ArkanoidGame?.OnPaint(sender, e);
        }

    }
}
