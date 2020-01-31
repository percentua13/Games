using System.Windows.Forms;
using System.Drawing;
using System;

namespace CheckersGame
{
    class Map
    {
        public const int m_MapSize = 8;
        public const int m_CellSize = 50;
        private int[,] m_Map;
        private Button[,] Buttons;

        private Button m_PreviousButton = null;

        bool m_IsMoving;

        private int m_CurrentPlayer;
        public Map()
        {
            m_Map = new int[m_MapSize, m_MapSize]
            {
                {0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 0, 1, 0, 1, 0},
                {0, 1, 0, 1, 0, 1, 0, 1},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {2, 0, 2, 0, 2, 0, 2, 0},
                {0, 2, 0, 2, 0, 2, 0, 2},
                {2, 0, 2, 0, 2, 0, 2, 0},
            };
            m_CurrentPlayer = 1;
            m_IsMoving = false;
            m_PreviousButton = null;
            
        }

        public Button[,] MapInitButtons()
        {
            Buttons = new Button[m_MapSize, m_MapSize];

            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    Buttons[i, j] = new Button();
                    Buttons[i, j].Location = new Point(j * m_CellSize, i * m_CellSize);
                    Buttons[i, j].Size = new Size(m_CellSize, m_CellSize);
                    Buttons[i, j].Click += new EventHandler(OnFigurePress);

                    switch(m_Map[i,j])
                    {
                        case 1:
                            Buttons[i, j].Image = (new Figure()).m_WhiteFigure;
                            break;
                        case 2:
                            Buttons[i, j].Image = (new Figure()).m_BlackFigure;
                            break;
                    }

                    if ((i % 2 != 0 && j % 2 == 0) || (i % 2 == 0 && j % 2 != 0))
                        Buttons[i, j].BackColor = (new Figure()).m_White;
                    else
                        Buttons[i, j].BackColor = (new Figure()).m_Black;

                }
            }

            return Buttons;
        }


        private void SwitchPlayer()
        {
            m_CurrentPlayer = m_CurrentPlayer == 1 ? 2 : 1;
        }

        private void OnFigurePress(object sender, EventArgs e)
        {
            if (m_PreviousButton != null)
                 m_PreviousButton.BackColor = GetPreviousButtonBackgroundColor();

            Button PressedButton = sender as Button;

            int i = PressedButton.Location.Y / m_CellSize,
                j = PressedButton.Location.X / m_CellSize;

            if (m_Map[i, j] == m_CurrentPlayer)
            {
                PressedButton.BackColor = Color.Red;
            }

            m_PreviousButton = PressedButton;

        }

        private Color GetPreviousButtonBackgroundColor()
        {
            int i = m_PreviousButton.Location.Y / m_CellSize,
                j = m_PreviousButton.Location.X / m_CellSize;

            if ((i % 2 != 0 && j % 2 == 0) || (i % 2 == 0 && j % 2 != 0))
                return (new Figure()).m_White;
            else
                return (new Figure()).m_Black;
        }
    }
}