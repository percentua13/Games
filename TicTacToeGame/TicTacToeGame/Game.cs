using System;
using System.Windows.Forms;
using System.Drawing;


namespace TicTacToeGame
{
    class Game
    {
        public const int m_MapSize = 3;
        public const int m_CellSize = 100;
        int[,] m_Map;
        Button[,] m_Buttons;
        int m_Player;
        public Game(Form1 form)
        {
            Initialization(form);
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
        public void Initialization(Form1 form)
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

                    form.Controls.Add(m_Buttons[i, j]);
                }
            }

            form.Size = new Size(m_MapSize * m_CellSize + 16, m_MapSize * m_CellSize + 40);
            form.MinimumSize = form.MaximumSize = form.Size;
            form.ActiveControl = null;

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

        private void SwitchPlayer() 
        {
            m_Player = m_Player == 1 ? 2 : 1;
        }
        private void OnCellPressed(object sender, EventArgs e)
        {         
            Button button = sender as Button;

            switch (m_Player)
            {
                case 1 :
                    button.Text = "X";
                    m_Map[button.Location.Y / m_CellSize, button.Location.X / m_CellSize] = 1;
                    SwitchPlayer();
                    break;

                case 2 :
                    button.Text = "O";
                    m_Map[button.Location.Y / m_CellSize, button.Location.X / m_CellSize] = 2;
                    SwitchPlayer();
                    break;
            }

            BlockAllNotEmptyCells();
        }

        int WhoIsWin()
        {
            int[] CounterForFirstPlayer = new int[4];
            int[] CounterForSecondPlayer = new int[4];


            for (int i = 0; i < m_MapSize; ++i)
            {

                for (int j = 0; j < m_MapSize; ++j)
                {
                    //Check rows
                    if (m_Map[i, j] == 1) ++CounterForFirstPlayer[0];                  
                    if (m_Map[i, j] == 2) ++CounterForSecondPlayer[0];

                    //Check columns
                    if (m_Map[j, i] == 1) ++CounterForFirstPlayer[1];
                    if (m_Map[j, i] == 2) ++CounterForSecondPlayer[1];

                }

                //Check main diagonal elements              
                if (m_Map[i, i] == 1) ++CounterForFirstPlayer[2];
                if (m_Map[i, i] == 2) ++CounterForSecondPlayer[2];

                //Check second diagonal elements
                if (m_Map[i, m_MapSize - i - 1] == 1) ++CounterForFirstPlayer[3];
                if (m_Map[i, m_MapSize - i - 1] == 2) ++CounterForSecondPlayer[3];

                
                if (CounterForFirstPlayer[0] == m_MapSize || CounterForFirstPlayer[1] == m_MapSize) 
                    return 1;
               
                if (CounterForSecondPlayer[0] == m_MapSize || CounterForSecondPlayer[1] == m_MapSize) 
                    return 2;

                //Clear data for Rows and Columns
                for (int k = 0; k < 2; ++k)
                {
                    if (CounterForFirstPlayer[k] == m_MapSize)
                        return 1;

                    if (CounterForSecondPlayer[k] == m_MapSize)
                        return 2;
                    CounterForFirstPlayer[k] = CounterForSecondPlayer[k] = 0;
                }
             
            }
        }

    }
}
