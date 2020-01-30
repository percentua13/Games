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
        public Game(ref Label lbl_Score, out int FormWidth, out int FormHeight)
        {
            #region

            //FORM
            FormWidth = (int)ENUM_DefinedNumbersForGame.LEFT_MARGIN + Map.m_MapWidth * (int)ENUM_DefinedNumbersForGame.PIXEL_SIZE + 19;
            FormHeight = Map.m_MapHeight * (int)ENUM_DefinedNumbersForGame.PIXEL_SIZE + 48;

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
            #region
            g.DrawRectangle(Pens.Black, new Rectangle((int)ENUM_DefinedNumbersForGame.LEFT_MARGIN + 0, 0, Map.m_MapWidth * (int)ENUM_DefinedNumbersForGame.PIXEL_SIZE, Map.m_MapHeight * (int)ENUM_DefinedNumbersForGame.PIXEL_SIZE));
            #endregion
        }
        private void DrawMap(Graphics g)
        {
            #region
            map.DrawMap(g);
            #endregion
        }

        public void OnPaint(object sender, PaintEventArgs e)
        {
            #region
            DrawMap(e.Graphics);
            DrawArea(e.Graphics);
            #endregion
        }
        public bool UpdateGameInfo(object sender, EventArgs e)
        {
            #region
            if (!map.IfMapCanBeApdate(ref Score))
            {
                bool answ = map.CheckIfWin();
                InitalizationParametersForNewGame();
                return answ;
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
