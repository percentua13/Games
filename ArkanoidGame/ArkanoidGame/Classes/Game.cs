using ArkanoidGame.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace ArkanoidGame
{
    class Game : IGame
    {
        #region fields
        private readonly IMap map;

        private int Score  = 0;

        private readonly Label lbl_Score;
        #endregion
        public Game(ArkanoidForm form, ref Label lbl_Score) 
        {
            #region
      
            form.Width = (int)ENUM_Properities.LEFT_MARGIN + Map.m_MapWidth * (int)ENUM_Properities.PIXEL_SIZE + 19;
            form.Height = Map.m_MapHeight * (int)ENUM_Properities.PIXEL_SIZE + (int)ENUM_Properities.BOTTOM_MARGIN;

            form.MaximumSize = new Size(form.Width, form.Height);
            form.MinimumSize = new Size(form.Width, form.Height);


            this.lbl_Score = lbl_Score;
            
            map = new Map();

            InitalizationParametersForNewGame();
            #endregion
        }
        private void InitalizationParametersForNewGame()
        {
            #region
            Score = 0;

            lbl_Score.Text = "Score : " + Score.ToString();
             
            map?.SetInitialProperities();
            #endregion
        }
        private void DrawArea(Graphics g)
        {
            g.DrawRectangle(Pens.Black, new Rectangle((int)ENUM_Properities.LEFT_MARGIN + 0, 0, Map.m_MapWidth * (int)ENUM_Properities.PIXEL_SIZE, Map.m_MapHeight * (int)ENUM_Properities.PIXEL_SIZE));
        }
        private void DrawMap(Graphics g)
        {
            map.DrawMap(g);
        }

        public void OnPaint(object sender, PaintEventArgs e)
        {
            DrawMap(e.Graphics);
            DrawArea(e.Graphics);
        }
        public bool UpdateGameInfo(object sender, EventArgs e)
        {
            #region
            if (!map.IfMapCanBeApdate(ref Score))
            {
                bool answer = map.CheckIfWin();
                InitalizationParametersForNewGame();
                return answer;
            }

            lbl_Score.Text = $"Score : {Score}";

            return false;
            #endregion
        }
        public void KeyInputCheck(object sender, KeyEventArgs e)
        {
            #region
            map.KeyInputCheck(sender, e);
            #endregion
        }
    }
}
