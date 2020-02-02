using System;
using System.Windows.Forms;

namespace TicTacToeGame
{
    class GameProcessWithBot : GameProcessAbstract
    {
        delegate void BotStepsDelegate();

        private int m_BotNumber;
        private static int NumberOfStep;

        BotStepsDelegate[] BotSteps;
        public override void StartNewGameProcess(Panel panel)
        {
            base.InitializationGameProcess(panel);

            m_BotNumber = new Random().Next(1, 10) % 2 == 0 ? 2 : 1;
            NumberOfStep = 1;

            BotSteps = new BotStepsDelegate[10];

            BotSteps[1] += Step1;
            BotSteps[2] += Step2;
            BotSteps[3] += Step3;
            BotSteps[4] += Step4;

            BotSteps[5] += Step5;
            BotSteps[6] += Step6;
            BotSteps[7] += Step5;
            BotSteps[8] += Step6;
            BotSteps[9] += Step5;

            if (m_BotNumber == 1) BotSteps[NumberOfStep++]();
            BlockAllNotEmptyCells();
        }

        public override void OnCellPressed(object sender, EventArgs e)
        {
            Button button = sender as Button;

            switch (NumberOfStep % 2)
            {
                case 1:
                    button.Text = "X";
                    m_Map[button.Location.Y / m_CellSize, button.Location.X / m_CellSize] = 1;
                    SwitchPlayer();
                    break;

                case 0:
                    button.Text = "O";
                    m_Map[button.Location.Y / m_CellSize, button.Location.X / m_CellSize] = 2;
                    SwitchPlayer();
                    break;
            }

            int Winner = WhoIsWin();

            if (Winner == 0)
            {
                BotSteps[++NumberOfStep]();
                ++NumberOfStep;
                BlockAllNotEmptyCells();
            }

            Winner = WhoIsWin();

            if (Winner == 0)
                return;

            BlockAllCells();

            if (Winner == -1)
                MessageBox.Show("Draw!");
            else
                MessageBox.Show($"{Winner} player has won! c:");
        }
       
        #region Steps (After 6 => repeat 5-6 steps)
        void Step1()
        {
            m_Buttons[1, 1].Text = "X";
            m_Map[1, 1] = 1;
        }

        void Step2()
        {
            #region
            if (m_Map[1, 1] == 0)
            {
                m_Buttons[1, 1].Text = "O";
                m_Map[1, 1] = 2;
                return;
            }

            int RandomDiagonalPosition = new Random().Next(1, 4);

            switch (RandomDiagonalPosition)
            {
                case 1:
                    if (m_Map[0, 0] != 0) break;
                    m_Buttons[0, 0].Text = "O";
                    m_Map[0, 0] = 2;
                    break;
                case 2:
                    if (m_Map[0, 2] != 0) break;
                    m_Buttons[0, 2].Text = "O";
                    m_Map[0, 2] = 2;
                    break;
                case 3:
                    if (m_Map[2, 0] != 0) break;
                    m_Buttons[2, 0].Text = "O";
                    m_Map[2, 0] = 2;
                    break;
                case 4:
                    if (m_Map[2, 2] != 0) break;
                    m_Buttons[2, 2].Text = "O";
                    m_Map[2, 2] = 2;
                    break;
            }
            #endregion
        }

        void Step3()
        {
            if (NeedsToCloseNotToLose(2, "X")) return;

            #region 1 0
            if (m_Map[1, 0] == 2)
            {
                int RandomPosition = new Random().Next(1, 10) % 2 == 1 ? 1 : 2;

                switch (RandomPosition)
                {
                    case 1:
                        m_Buttons[0, 0].Text = "X";
                        m_Map[0, 0] = 1;
                        break;
                    case 2:
                        m_Buttons[2, 0].Text = "X";
                        m_Map[2, 0] = 1;
                        break;
                }
                return;
            }
            #endregion

            #region 0 1
            if (m_Map[0, 1] == 2)
            {
                int RandomPosition = new Random().Next(1, 10) % 2 == 1 ? 1 : 2;

                switch (RandomPosition)
                {
                    case 1:
                        m_Buttons[0, 0].Text = "X";
                        m_Map[0, 0] = 1;
                        break;
                    case 2:
                        m_Buttons[0, 2].Text = "X";
                        m_Map[0, 2] = 1;
                        break;
                }
                return;
            }

            if (m_Map[1, 2] == 2)
            {
                int RandomPosition = new Random().Next(1, 10) % 2 == 1 ? 1 : 2;

                switch (RandomPosition)
                {
                    case 1:
                        m_Buttons[0, 2].Text = "X";
                        m_Map[0, 2] = 1;
                        break;
                    case 2:
                        m_Buttons[2, 2].Text = "X";
                        m_Map[2, 2] = 1;
                        break;
                }
                return;
            }
            #endregion

            #region 1 2
            if (m_Map[1, 2] == 2)
            {
                int RandomPosition = new Random().Next(1, 10) % 2 == 1 ? 1 : 2;

                switch (RandomPosition)
                {
                    case 1:
                        m_Buttons[2, 0].Text = "X";
                        m_Map[2, 0] = 1;
                        break;
                    case 2:
                        m_Buttons[2, 2].Text = "X";
                        m_Map[2, 2] = 1;
                        break;
                }
                return;
            }
            #endregion

            #region 2 1
            if (m_Map[2, 1] == 2)
            {
                int RandomPosition = new Random().Next(1, 10) % 2 == 1 ? 1 : 2;

                switch (RandomPosition)
                {
                    case 1:
                        m_Buttons[2, 0].Text = "X";
                        m_Map[2, 0] = 1;
                        break;
                    case 2:
                        m_Buttons[2, 2].Text = "X";
                        m_Map[2, 2] = 1;
                        break;
                }
                return;
            }
            #endregion

            bool IsContinue = true;
            while (IsContinue)
            {
                int RandomDiagonalPosition = new Random().Next(1, 4);
                switch (RandomDiagonalPosition)
                {
                    case 1:
                        if (m_Map[0, 0] != 0 || m_Map[2, 2] != 0) break; 
                        m_Buttons[0, 0].Text = "X";
                        m_Map[0, 0] = 1;
                        IsContinue = false;
                        break;
                    case 2:
                        if (m_Map[2, 0] != 0 || m_Map[0, 2] != 0) break;
                        m_Buttons[0, 2].Text = "X";
                        m_Map[0, 2] = 1;
                        IsContinue = false;
                        break;
                    case 3:
                        if (m_Map[2, 0] != 0 || m_Map[0, 2] != 0) break;
                        m_Buttons[2, 0].Text = "X";
                        m_Map[2, 0] = 1;
                        IsContinue = false;
                        break;
                    case 4:
                        if (m_Map[0, 0] != 0 || m_Map[2, 2] != 0) break;
                        m_Buttons[2, 2].Text = "X";
                        m_Map[2, 2] = 1;
                        IsContinue = false;
                        break;
                }
            }
        }

        void Step4()
        {
            if (NeedsToCloseNotToLose(1, "O")) return;
            NextBotStep(2, "O", 1);
        }
        void Step5()
        {
            if (NeedsToCloseToWin(1, "X")) return;
            if (NeedsToCloseNotToLose(2, "X")) return;
            NextBotStep(1, "X", 1);
        }
        void Step6()
        {
            if (NeedsToCloseToWin(2, "O")) return;
            if (NeedsToCloseNotToLose(1, "O")) return;
            NextBotStep(2, "O", 1);
        }
        #endregion
        bool NeedsToCloseToWin(int GamerNumber, string Close)
        {
            return NextBotStep(GamerNumber, Close, 2);
        }

        bool NeedsToCloseNotToLose(int GamerNumber, string Close)
        {
            //int Gamer = GamerNumber == 1 ? 2 : 1;

            return NextBotStep(GamerNumber, Close, 2);
        }
  
        bool NextBotStep(int GamerNumber, string Close, int CountOfFilled)
        {
            int Gamer = Close == "X" ? 1 : 2;

            #region 0 0
            if (m_Map[0, 0] == 0)
                if (CheckRow(0, GamerNumber) == CountOfFilled || CheckColumn(0, GamerNumber) == CountOfFilled ||
                    CheckMainDiagonal(GamerNumber) == CountOfFilled)
                {
                    m_Map[0, 0] = Gamer;
                    m_Buttons[0, 0].Text = Close;
                    return true;
                }
            #endregion

            #region 0 1
            if (m_Map[0, 1] == 0)
                if (CheckRow(0, GamerNumber) == CountOfFilled || CheckColumn(1, GamerNumber) == CountOfFilled)
                {
                    m_Map[0, 1] = Gamer;
                    m_Buttons[0, 1].Text = Close;
                    return true;
                }
            #endregion

            #region 0 2
            if (m_Map[0, 2] == 0)
                if (CheckRow(0, GamerNumber) == CountOfFilled || CheckColumn(2, GamerNumber) == CountOfFilled ||
                CheckSecondDiagonal(GamerNumber) == CountOfFilled)
                {
                    m_Map[0, 2] = Gamer;
                    m_Buttons[0, 2].Text = Close;
                    return true;
                }
            #endregion

            #region 1 0
            if (m_Map[1, 0] == 0)
                 if (CheckRow(1, GamerNumber) == CountOfFilled || CheckColumn(0, GamerNumber) == CountOfFilled)
                 {
                    m_Map[1, 0] = Gamer;
                    m_Buttons[1, 0].Text = Close;
                    return true;
                 }
            #endregion
            
            #region 1 1
            if (m_Map[1, 1] == 0)
                if (CheckRow(1, GamerNumber) == CountOfFilled || CheckColumn(1, GamerNumber) == CountOfFilled ||
                    CheckMainDiagonal(GamerNumber) == CountOfFilled || CheckSecondDiagonal(GamerNumber) == CountOfFilled)
                {
                    m_Map[1, 1] = Gamer;
                    m_Buttons[1, 1].Text = Close;
                    return true;
                }
            #endregion

            #region 1 2
            if (m_Map[1, 2] == 0)
                if (CheckRow(1, GamerNumber) == CountOfFilled || CheckColumn(2, GamerNumber) == CountOfFilled)
                {
                    m_Map[1, 2] = Gamer;
                    m_Buttons[1, 2].Text = Close;
                    return true;
                }
            #endregion

            #region 2 0
            if (m_Map[2, 0] == 0)
                if (CheckRow(2, GamerNumber) == CountOfFilled || CheckColumn(0, GamerNumber) == CountOfFilled ||
                CheckSecondDiagonal(GamerNumber) == CountOfFilled)
                {
                    m_Map[2, 0] = Gamer;
                    m_Buttons[2, 0].Text = Close;
                    return true;
                }
            #endregion

            #region 2 1
            if (m_Map[2, 1] == 0)
                if (CheckRow(2, GamerNumber) == CountOfFilled || CheckColumn(1, GamerNumber) == CountOfFilled)
                {
                    m_Map[2, 1] = Gamer;
                    m_Buttons[2, 1].Text = Close;
                    return true;
                }
            #endregion

            #region 2 2
            if (m_Map[2, 2] == 0)
                if (CheckRow(2, GamerNumber) == CountOfFilled || CheckColumn(2, GamerNumber) == CountOfFilled ||
                CheckMainDiagonal(GamerNumber) == CountOfFilled)
                {
                    m_Map[2, 2] = Gamer;
                    m_Buttons[2, 2].Text = Close;
                    return true;
                }
            #endregion
            return false;
        }

        #region Checks
        int CheckRow(int i, int Value)
        {
            int Count = 0;
            for (int j = 0; j < m_MapSize; ++j)
            {
                if (m_Map[i,j] == Value)
                ++Count;
            }
            return Count;
        }

        int CheckColumn(int j, int Value)
        {
            int Count = 0;
            for (int i = 0; i < m_MapSize; ++i)
            {
                if (m_Map[i, j] == Value)
                    ++Count;
            }
            return Count;
        }

        int CheckMainDiagonal(int Value)
        {
            int Count = 0;

            if (m_Map[0, 0] == Value)
                ++Count;
            if (m_Map[1, 1] == Value)
                ++Count;
            if (m_Map[2, 2] == Value)
                ++Count;

            return Count;
        }

        int CheckSecondDiagonal(int Value)
        {
            int Count = 0;

            if (m_Map[0, 2] == Value)
                ++Count;
            if (m_Map[1, 1] == Value)
                ++Count;
            if (m_Map[2, 0] == Value)
                ++Count;

            return Count;
        }
        #endregion
    }
}
