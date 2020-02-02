using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToeGame
{
    public partial class TicTacToe : Form
    {
        public TicTacToe()
        {
            InitializeComponent();

           Game game = new Game();

           game.StartNewGame(this);
        }


    }
}
