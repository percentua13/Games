using System;
using System.Windows.Forms;

namespace TicTacToeGame
{
    class GameProcessSimple : GameProcessAbstract
    {
        public override void OnCellPressed(object sender, EventArgs e)
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
    }
}
