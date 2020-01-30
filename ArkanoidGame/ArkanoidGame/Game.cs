using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    enum DefinedNumbersForGame
    {
        BALL = 88,
        SET1 = 99,
        SET2 = 999,
        SET3 = 9999,
        BLOCK1 = 1,
        BLOCK2 = 11,
    }
    class Game
    {
        public const int PixelSize = 20;

        private Map map;

        public int Score  = 0;

        private /*readonly*/ Label lbl_Score;

        private Form form;

        private Timer TimerForGame;
        public Game(Form form, ref Timer timer)
        {
            lbl_Score = new Label() { Text = "Score : " + Score.ToString() };
            lbl_Score.Location = new Point(Map.mapWidth * PixelSize + 5, (int)(Map.mapHeight / 2.5 * PixelSize));

            form.Controls.Add(lbl_Score);

            this.form = form;
            
            TimerForGame = timer;

            TimerForGame.Interval = 60;

            
            map = new Map();

            Init();
        }
        internal void Init()
        {
            Score = 0;

            lbl_Score.Text = "Score : " + Score.ToString();

            //FORM
            form.Width = Map.mapWidth * PixelSize + 100;
            form.Height = Map.mapHeight * PixelSize + 48;      
           
            map?.SetInitProperities();
        }

        internal void OnPaint(object sender, PaintEventArgs e)
        {
            DrawMap(e.Graphics);
            DrawArea(e.Graphics);
        }

        public void DrawArea(Graphics g)
        {
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, Map.mapWidth * PixelSize, Map.mapHeight * PixelSize));
        }
        public void DrawMap(Graphics g)
        {
            map.DrawMap(g, this);
        }

        internal void update(object sender, EventArgs e)
        {
            map.update(sender, e, this);

            lbl_Score.Text = "Score : " + Score.ToString();
        }
        internal void InputCheck(object sender, KeyEventArgs e)
        {
            map.InputCheck(sender, e);
        }
    }
}
