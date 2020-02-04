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

            this.Controls.Add(lbl_Info);

        }
    }
}
