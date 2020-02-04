using System;
using System.Windows.Forms;
using System.Drawing;


namespace TicTacToeGame
{
    class Game
    {
        TicTacToe form;
        GameProcessAbstract game = new GameProcessSimple();

        public void StartNewGame(TicTacToe form)
        {
            Initialization(form);
        }
        public void Initialization(TicTacToe form)
        {
            this.form = form;
            form.Controls.Clear();

            MenuStrip menu = new MenuStrip();
            Panel panel = new Panel();

            form.Controls.Add(menu);
            form.Controls.Add(panel);


            SetPanelProperities(panel);
            SetMenuProperities(menu);

            panel.Location = new Point(0, menu.Height);

            game.StartNewGameProcess(panel);


            form.Size = new Size(panel.Width, panel.Height + menu.Height - 1);
            form.MinimumSize = form.MaximumSize = form.Size;
        }
        private void SetPanelProperities(Panel panel)
        {
            panel.Size = new Size(GameProcessAbstract.m_MapSize * GameProcessAbstract.m_CellSize + 16, GameProcessAbstract.m_MapSize * GameProcessAbstract.m_CellSize + 40);
            panel.MinimumSize = panel.MaximumSize = panel.Size;
            panel.Location = new Point(0, 25);
        }

        private void SetMenuProperities(MenuStrip menu)
        {
            menu.Size = new Size(GameProcessAbstract.m_MapSize * GameProcessAbstract.m_CellSize + 16, 25);
            menu.MinimumSize = menu.MaximumSize = menu.Size;

            menu.Font = new Font("OCR A", 12, GraphicsUnit.Pixel);


            ToolStripMenuItem NewGameItem = new ToolStripMenuItem("Start new game");

            ToolStripMenuItem NewGameBotItem = new ToolStripMenuItem("Start new game with bot");

            menu.Items.Add(NewGameItem);
            menu.Items.Add(NewGameBotItem);

            NewGameItem.Click += OnMenuClickSimpleGame;
            NewGameBotItem.Click += OnMenuClickGameWithBot;

            menu.Location = new Point(0, 0);
            menu.BackColor = Color.White;
            menu.AutoSize = true;
        }

        private void OnMenuClickSimpleGame(object sender, EventArgs e)
        {
            game = new GameProcessSimple();
            StartNewGame(form);
        }

        private void OnMenuClickGameWithBot(object sender, EventArgs e)
        {
            game = new GameProcessWithBot();
            StartNewGame(form);
        }

    }
}
