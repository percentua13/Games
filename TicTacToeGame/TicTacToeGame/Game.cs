using System;
using System.Windows.Forms;
using System.Drawing;


namespace TicTacToeGame
{
    class Game
    {
        Form1 form;
        GameProcess game;

        public void StartNewGame(Form1 form)
        {
            Initialization(form);
        }
        public void Initialization(Form1 form)
        {
            this.form = form;
            form.Controls.Clear();

            MenuStrip menu = new MenuStrip();
            Panel panel = new Panel();
            form.Controls.Add(menu);
            form.Controls.Add(panel);


            SetPanelProperities(panel);
            SetMenuProperities(menu);
            game = new GameProcess(panel);


            form.Size = new Size(panel.Width, panel.Height + menu.Height);
            form.MinimumSize = form.MaximumSize = form.Size;
        }
        private void SetPanelProperities(Panel panel)
        {
            panel.Size = new Size(GameProcess.m_MapSize * GameProcess.m_CellSize + 16, GameProcess.m_MapSize * GameProcess.m_CellSize + 40);
            panel.MinimumSize = panel.MaximumSize = panel.Size;
            panel.Location = new Point(0, 25);
        }

        private void SetMenuProperities(MenuStrip menu)
        {
            menu.Size = new Size(GameProcess.m_MapSize * GameProcess.m_CellSize + 16, 10);
            menu.MinimumSize = menu.MaximumSize = menu.Size;

            ToolStripMenuItem NewGameItem = new ToolStripMenuItem("Start new game");

            ToolStripMenuItem NewGameBotItem = new ToolStripMenuItem("Start new game with bot");

            menu.Items.Add(NewGameItem);
            menu.Items.Add(NewGameBotItem);

            NewGameItem.Click += OnMenuClick;
            
        }

        private void OnMenuClick(object sender, EventArgs e)
        {
                StartNewGame(form);
        }
     
    }
}
