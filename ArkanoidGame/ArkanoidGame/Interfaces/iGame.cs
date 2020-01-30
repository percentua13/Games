using System;
using System.Windows.Forms;

namespace ArkanoidGame.Interfaces
{
    interface IGame
    {
        public void OnPaint(object sender, PaintEventArgs e);
        public bool UpdateGameInfo(object sender, EventArgs e);
        public void KeyInputCheck(object sender, KeyEventArgs e);
    }
}
