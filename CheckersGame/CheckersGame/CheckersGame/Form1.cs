using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        Map GameMap;

        public void Init()
        {
            this.Height = Map.m_CellSize * Map.m_MapSize + 39;
            this.Width = Map.m_CellSize * Map.m_MapSize + 16;
            this.MaximumSize = new Size(this.Width, this.Height);
            this.MinimumSize = new Size(this.Width, this.Height);

            GameMap = new Map(this);
            Button [,] buttons = GameMap.MapInitButtons();
            for (int i = 0; i < buttons.GetLength(0); ++i)
            {
                for (int j=0; j< buttons.GetLength(1); ++j)
                {
                    this.Controls.Add(buttons[i, j]);
                }
            }
        }
    }
}
