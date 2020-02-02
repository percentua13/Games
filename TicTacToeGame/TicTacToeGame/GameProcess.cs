using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToeGame
{
    class GameProcess
    {
        public const int m_MapSize = 3;
        public const int m_CellSize = 100;
        int[,] m_Map;
        Button[,] m_Buttons;
        int m_Player;

        public GameProcess(Panel panel)
        {
            InitializationGameProcess(panel);
        }

        private void InitializationGameProcess(Panel panel)
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
        private void SetButtonProperity(Button button)
        {
            button.Size = new Size(m_CellSize, m_CellSize);

            button.FlatAppearance.BorderSize = 1;
            button.FlatStyle = FlatStyle.Flat;

            button.BackColor = Color.White;
            button.Font = new Font(FontFamily.GenericMonospace, 50, FontStyle.Bold, GraphicsUnit.Pixel);
            // m_Buttons[i, j].FlatAppearance.MouseDownBackColor = Color.Yellow;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 255, 187);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 200, 0);


            //m_Buttons[i, j].Cl
            button.Click += new EventHandler(OnCellPressed);

        }



        private void BlockAllNotEmptyCells()
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

        private void BlockAllCells()
        {
            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    m_Buttons[i, j].Enabled = false;
                }
            }
        }

        private void SwitchPlayer()
        {
            m_Player = m_Player == 1 ? 2 : 1;
        }

        private void OnCellPressed(object sender, EventArgs e)
        {
            Button button = sender as Button;

            switch (m_Player)
            {
                case 1:
                    button.Text = "X";
                    m_Map[button.Location.Y / m_CellSize, button.Location.X / m_CellSize] = 1;
                    SwitchPlayer();
                    break;

                case 2:
                    button.Text = "O";
                    m_Map[button.Location.Y / m_CellSize, button.Location.X / m_CellSize] = 2;
                    SwitchPlayer();
                    break;
            }

            BlockAllNotEmptyCells();

            int Winner = WhoIsWin();
            if (Winner == 0) return;
            BlockAllCells();
            if (Winner == -1)
                MessageBox.Show("Draw!");
            else
                MessageBox.Show($"{Winner} player has won! c:");


        }

        int WhoIsWin()
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

            switch (NumberOfCase)
            {
                case 0:
                    for (int j = 0; j < m_MapSize; ++j)
                        m_Buttons[StartIndex, j].BackColor = Color.Red;
                    break;
                case 1:
                    for (int i = 0; i < m_MapSize; ++i)
                        m_Buttons[i, StartIndex].BackColor = Color.Red;
                    break;
                case 2:
                    for (int i = 0; i < m_MapSize; ++i)
                        m_Buttons[i, i].BackColor = Color.Red;
                    break;
                case 3:
                    for (int i = 0; i < m_MapSize; ++i)
                        m_Buttons[i, m_MapSize - i - 1].BackColor = Color.Red;
                    break;
            }
        }
}
}
