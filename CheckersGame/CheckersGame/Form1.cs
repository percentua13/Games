using System.Drawing;
using System.Windows.Forms;

namespace CheckersGame
{
    public partial class CheckersForm : Form
    {
        public CheckersForm()
        {
            InitializeComponent();
            Init();
        }

        Map GameMap;
        public Label lbl_Info;

        public void Init()
        {
            this.Size = new Size(Map.m_CellSize * Map.m_MapSize + 16, Map.m_CellSize * Map.m_MapSize + 80);
            this.MinimumSize = this.MaximumSize = this.Size;

            lbl_Info = new Label();

            GameMap = new Map(this);

            Button [,] buttons = GameMap.MapInitButtons();
            for (int i = 0; i < buttons.GetLength(0); ++i)
            {
                for (int j=0; j < buttons.GetLength(1); ++j)
                {
                    this.Controls.Add(buttons[i, j]);
                }
            }

            this.Controls.Add(lbl_Info);

        }
    }
}
