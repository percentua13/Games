using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToeGame
{
    abstract class GameProcessAbstract
    {
        public const int m_MapSize = 3;
        public const int m_CellSize = 100;
        protected int[,] m_Map;
        protected Button[,] m_Buttons;
        protected int m_Player;

        public virtual void StartNewGameProcess(Panel panel)
        {
            InitializationGameProcess(panel);
        }

        virtual protected void InitializationGameProcess(Panel panel)
        {
            m_Map = new int[m_MapSize, m_MapSize];
            m_Buttons = new Button[m_MapSize, m_MapSize];
            m_Player = 1;

            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    m_Buttons[i, j] = new Button();
                    m_Buttons[i, j].Location = new Point(j * m_CellSize, i * m_CellSize);

                    SetButtonProperity(m_Buttons[i, j]);

                    panel.Controls.Add(m_Buttons[i, j]);
                }
            }
        }
        
        protected void SetButtonProperity(Button button)
        {
            button.Size = new Size(m_CellSize, m_CellSize);

            button.FlatAppearance.BorderSize = 2;
            button.FlatStyle = FlatStyle.Flat;

            button.BackColor = Color.White;
            button.Font = new Font(FontFamily.GenericMonospace, 50, FontStyle.Bold, GraphicsUnit.Pixel);

            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(219, 253, 206);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 200, 0);


            button.Click += new EventHandler(OnCellPressed);

        }

        protected void BlockAllNotEmptyCells()
        {
            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    if (m_Buttons[i, j].Text != "")
                        m_Buttons[i, j].Enabled = false;
                }
            }
        }

        //When game is finished
        protected void BlockAllCells()
        {
            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    m_Buttons[i, j].Enabled = false;
                }
            }
        }

        protected void SwitchPlayer()
        {
            m_Player = m_Player == 1 ? 2 : 1;
        }

        public abstract void OnCellPressed(object sender, EventArgs e);
       
        protected int WhoIsWin()
        {
            #region
            int[] CounterForFirstPlayer = new int[4];
            int[] CounterForSecondPlayer = new int[4];

            int CountOfNotEmptyCells = 0;

            for (int i = 0; i < m_MapSize; ++i)
            {
                #region
                for (int j = 0; j < m_MapSize; ++j)
                {
                    //Check rows
                    if (m_Map[i, j] == 1) ++CounterForFirstPlayer[0];
                    if (m_Map[i, j] == 2) ++CounterForSecondPlayer[0];

                    //Check columns
                    if (m_Map[j, i] == 1) ++CounterForFirstPlayer[1];
                    if (m_Map[j, i] == 2) ++CounterForSecondPlayer[1];

                    if (m_Map[i, j] != 0) ++CountOfNotEmptyCells;
                }

                //Check main diagonal elements              
                if (m_Map[i, i] == 1) ++CounterForFirstPlayer[2];
                if (m_Map[i, i] == 2) ++CounterForSecondPlayer[2];

                //Check second diagonal elements
                if (m_Map[i, m_MapSize - i - 1] == 1) ++CounterForFirstPlayer[3];
                if (m_Map[i, m_MapSize - i - 1] == 2) ++CounterForSecondPlayer[3];


                for (int k = 0; k < 2; ++k)
                {
                    //Check if someone win
                    if (CounterForFirstPlayer[k] == m_MapSize)
                    {
                        ShowWinCells(i, k);
                        return 1;
                    }
                    if (CounterForSecondPlayer[k] == m_MapSize)
                    {
                        ShowWinCells(i, k);
                        return 2;
                    }

                    //Clear data
                    CounterForFirstPlayer[k] = CounterForSecondPlayer[k] = 0;
                }
                #endregion
            }

            #region
            //Check diagonal elements
            for (int k = 2; k < 4; ++k)
            {
                //Check if someone win
                if (CounterForFirstPlayer[k] == m_MapSize)
                {
                    ShowWinCells(0, k);
                    return 1;
                }

                if (CounterForSecondPlayer[k] == m_MapSize)
                {
                    ShowWinCells(0, k);
                    return 2;
                }
            }
            #endregion

            if (CountOfNotEmptyCells == Math.Pow(m_MapSize, 2)) return -1;
            return 0;
            #endregion
        }

        void ShowWinCells(int StartIndex, int NumberOfCase)
        {
            //0 - rows
            //1 - collumns
            //2 - main diagonal
            //3 - second diagonal
            Color ColorWin = Color.FromArgb(254, 180, 180);
            switch (NumberOfCase)
            {
                case 0:
                    for (int j = 0; j < m_MapSize; ++j)
                        m_Buttons[StartIndex, j].BackColor = ColorWin;
                    break;
                case 1:
                    for (int i = 0; i < m_MapSize; ++i)
                        m_Buttons[i, StartIndex].BackColor = ColorWin;
                    break;
                case 2:
                    for (int i = 0; i < m_MapSize; ++i)
                        m_Buttons[i, i].BackColor = ColorWin;
                    break;
                case 3:
                    for (int i = 0; i < m_MapSize; ++i)
                        m_Buttons[i, m_MapSize - i - 1].BackColor = ColorWin;
                    break;
            }
        }
    }
}
