using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame.Interfaces
{
    public interface IMap
    {
        public void DrawMap(Graphics g);
        public void SetInitialProperities();
        public bool IfMapCanBeApdate(ref int Score);
        public void KeyInputCheck(object sender, KeyEventArgs e);

        public bool CheckIfWin();
    }
}
