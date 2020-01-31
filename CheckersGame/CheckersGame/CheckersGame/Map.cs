using System.Windows.Forms;
using System.Drawing;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CheckersGame
{
    class Map
    {
        Form1 form;

        public const int m_MapSize = 8;
        public const int m_CellSize = 50;
        private int[,] m_Map;
        private Button[,] m_Buttons;

        private Button m_PreviousButton = null;
        Button m_PressedButton = null;

        bool m_IsMoving;

        List<Button> m_SimpleSteps = new List<Button>();

        int m_CountEatSteps = 0;
        bool m_IsContinue;
        private int m_CurrentPlayer;
        public Map(Form1 form)
        {
            #region
            m_Map = new int[m_MapSize, m_MapSize]
            {
                {0, 1, 0, 1, 0, 1, 0, 1},
                {1, 0, 1, 0, 1, 0, 1, 0},
                {0, 1, 0, 1, 0, 1, 0, 1},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {2, 0, 2, 0, 2, 0, 2, 0},
                {0, 2, 0, 2, 0, 2, 0, 2},
                {2, 0, 2, 0, 2, 0, 2, 0},
            };
            m_CurrentPlayer = 1;
            m_IsMoving = false;
            m_PreviousButton = null;

            this.form = form;
            #endregion

        }

        public Button[,] MapInitButtons()
        {
            #region
            m_Buttons = new Button[m_MapSize, m_MapSize];

            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    m_Buttons[i, j] = new Button();
                    m_Buttons[i, j].Location = new Point(j * m_CellSize, i * m_CellSize);
                    m_Buttons[i, j].Size = new Size(m_CellSize, m_CellSize);
                    m_Buttons[i, j].Click += new EventHandler(OnFigurePress);

                    switch (m_Map[i, j])
                    {
                        case 1:
                            m_Buttons[i, j].Image = (new Figure()).m_WhiteFigure;
                            break;
                        case 2:
                            m_Buttons[i, j].Image = (new Figure()).m_BlackFigure;
                            break;
                    }

                    if ((i % 2 != 0 && j % 2 == 0) || (i % 2 == 0 && j % 2 != 0))
                        m_Buttons[i, j].BackColor = (new Figure()).m_White;
                    else
                        m_Buttons[i, j].BackColor = (new Figure()).m_Black;

                }
            }

            return m_Buttons;
            #endregion
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        private void SwitchPlayer(Form1 form)
        {
            m_CurrentPlayer = m_CurrentPlayer == 1 ? 2 : 1;
            ResetGame();
        }


        public void ResetGame()
        {
            bool player1 = false;
            bool player2 = false;

            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    if (m_Map[i, j] == 1)
                        player1 = true;

                    if (m_Map[i, j] == 2)
                        player2 = true;
                }
            }

            if (!player1 || !player2)
            {
                if (!player1) MessageBox.Show("Win second player!");
                else MessageBox.Show("Win first player!");

                form.Controls.Clear();
                form.Init();
            }
        }
        private void OnFigurePress(object sender, EventArgs e)
        {
            m_PressedButton = sender as Button;

            if (m_PreviousButton != null)
                m_PreviousButton.BackColor = GetPreviousButtonBackgroundColor(m_PreviousButton);


            int i_pressed = m_PressedButton.Location.Y / m_CellSize,
                j_pressed = m_PressedButton.Location.X / m_CellSize;
               

            if (m_Map[i_pressed, j_pressed] == m_CurrentPlayer)
            {
                CloseSteps();
                m_PressedButton.BackColor = Color.Red;
                DeactivateAllButtons();

                m_PressedButton.Enabled = true;
                m_CountEatSteps = 0;

                if (m_PressedButton.Text == "D")
                    ShowSteps(i_pressed, j_pressed, false);
                else
                    ShowSteps(i_pressed, j_pressed);

                if (m_IsMoving)
                {
                    CloseSteps();
                    m_PressedButton.BackColor = GetPreviousButtonBackgroundColor(m_PressedButton);
                    ShowPossibleSteps();
                    m_IsMoving = false;
                }
                else 
                    m_IsMoving = true;
            }
            else 
            {
                if (m_IsMoving)
                {
                    int i_previous = m_PreviousButton.Location.Y / m_CellSize,
                        j_previous = m_PreviousButton.Location.X / m_CellSize;

                    m_IsContinue = false;
                                    
                    if (Math.Abs(j_pressed - j_previous) > 1)
                    {
                        m_IsContinue = true;
                        DeleteEaten(m_PressedButton, m_PreviousButton);
                    }

                    Swap(ref m_Map[i_pressed, j_pressed], ref m_Map[i_previous, j_previous]);

                    m_PressedButton.Image = m_PreviousButton.Image;
                    m_PressedButton.Text = m_PreviousButton.Text;

                    m_PreviousButton.Image = null;
                    m_PreviousButton.Text = "";

                    SwitchButtonToCheat(m_PressedButton);
                    m_CountEatSteps = 0;
                    m_IsMoving = false;

                    CloseSteps();
                    DeactivateAllButtons();

                    if (m_PressedButton.Text == "D")
                        ShowSteps(i_pressed, j_pressed, false);
                    else
                        ShowSteps(i_pressed, j_pressed);

                    if (m_CountEatSteps == 0 || !m_IsContinue)
                    {
                        CloseSteps();
                        SwitchPlayer(form);
                        ShowPossibleSteps();
                        m_IsContinue = false;
                    }
                    else if (m_IsContinue)
                    {
                        m_PressedButton.BackColor = Color.Red;
                        m_PressedButton.Enabled = true;
                        m_IsMoving = true;
                    }
                       
                }
            }

            m_PreviousButton = m_PressedButton;

        }

        private Color GetPreviousButtonBackgroundColor(Button m_PreviousButton)
        {
            int i = m_PreviousButton.Location.Y / m_CellSize,
                j = m_PreviousButton.Location.X / m_CellSize;

            if ((i % 2 != 0 && j % 2 == 0) || (i % 2 == 0 && j % 2 != 0))
                return (new Figure()).m_White;
            else
                return (new Figure()).m_Black;
        }



        private void ShowPossibleSteps()
        {
            bool IsOneStep = true;
            bool IsEatStep = false;

            DeactivateAllButtons();

            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    if (m_Map[i, j] == m_CurrentPlayer)
                    {
                        IsOneStep = m_Buttons[i, j].Text == "D" ? false : true;

                        if (IsButtonHasEatStep(i, j, IsOneStep, new int[2] { 0, 0 }))
                        {
                            IsEatStep = true;
                            m_Buttons[i, j].Enabled = true;
                        }
                    }
                }
            }

            if (!IsEatStep) ActivateAllButtons();
        }
        private void SwitchButtonToCheat(Button button)
        {
            int i = button.Location.Y / m_CellSize,
                j = button.Location.X / m_CellSize;

            if (m_Map[i,j] == 1 && i == m_MapSize - 1)
            {
                button.Text = "D";
            }

            if (m_Map[i, j] == 2 && i == 0)
            {
                button.Text = "D";
            }
        }
         
        private void DeleteEaten(Button EndButton, Button StartButton)
        {
            int Count = Math.Abs(EndButton.Location.Y / m_CellSize - StartButton.Location.Y / m_CellSize);

            int StartX = EndButton.Location.Y / m_CellSize - StartButton.Location.Y / m_CellSize,
                StartY = EndButton.Location.X / m_CellSize - StartButton.Location.X / m_CellSize;

            StartX = StartX < 0 ? -1 : 1;
            StartY = StartY < 0 ? -1 : 1;

            int CurrentCount = 0;

            int i = StartButton.Location.Y / m_CellSize + StartX, //!!!
                j = StartButton.Location.X / m_CellSize + StartY; //!!!

            while (CurrentCount < Count - 1)
            {
                m_Map[i, j] = 0;
                m_Buttons[i, j].Image = null;
                m_Buttons[i, j].Text = "";

                i += StartX;
                j += StartY;

                ++CurrentCount;
            }

        }

        private void ShowSteps(int iCurrent, int jCurrent, bool IsOneStep = true)
        {
            m_SimpleSteps.Clear();
            ShowDiagonal(iCurrent, jCurrent, IsOneStep);

            if (m_CountEatSteps > 0)
                CloseSimpleSteps(m_SimpleSteps);
        }


        private void ShowDiagonal(int iCurrent, int jCurrent, bool IsOneStep = false)
        {
            #region 1
            int j = jCurrent + 1;

            for (int i = iCurrent - 1; i >= 0; --i)
            {
             
                if (m_CurrentPlayer == 1)
                {
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, true)) break;
                }
                else 
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, false)) break;



                if (j < m_MapSize - 1) ++j;
                else break;

                if (IsOneStep) break;
            }
            #endregion
            #region 2
            j = jCurrent - 1;

            for (int i = iCurrent - 1; i >= 0; --i)
            {
                if (m_CurrentPlayer == 1 &&  i < iCurrent-2  && IsOneStep && !m_IsContinue) break;

                if (m_CurrentPlayer == 1)
                {
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, true)) break;
                }
                else
                   if (IsInsideBorder(i, j) && !DeterminePath(i, j, false)) break;


                if (j > 0) --j;
                else break;

                if (IsOneStep) break;
            }
            #endregion
            #region 3
            j = jCurrent - 1;

            for (int i = iCurrent + 1; i < m_MapSize; ++i)
            {
                if (m_CurrentPlayer == 2 && i > iCurrent + 2 && IsOneStep && !m_IsContinue) break;

                if (m_CurrentPlayer == 2)
                {
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, true)) break;
                }
                else
                   if (IsInsideBorder(i, j) && !DeterminePath(i, j, false)) break;


                if (j > 0) --j;
                else break;

                if (IsOneStep) break;
            }
            #endregion
            #region 4
            j = jCurrent + 1;

            for (int i = iCurrent + 1; i < m_MapSize; ++i)
            {
               if (m_CurrentPlayer == 2 && i > iCurrent + 2 && IsOneStep && !m_IsContinue) break;

                if (m_CurrentPlayer == 2)
                {
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, true)) break;
                }
                else
                 if (IsInsideBorder(i, j) && !DeterminePath(i, j, false)) break;

                if (j < m_MapSize - 1) ++j;
                else break;

                if (IsOneStep) break;
            }
            #endregion
        }


        private bool DeterminePath(int ti, int tj, bool BiteBack)
        {
            if (!BiteBack && m_Map[ti, tj] == 0 && !m_IsContinue)
            {
                m_Buttons[ti, tj].BackColor = Color.Yellow;
                m_Buttons[ti, tj].Enabled = true;
                m_SimpleSteps.Add(m_Buttons[ti, tj]);
            }
            else if (BiteBack || (!BiteBack && m_Map[ti, tj] != 0))
            {
                if (m_Map[ti, tj] != m_CurrentPlayer)
                {
                    if (m_PressedButton.Text == "D")
                        ShowProceduralEat(ti, tj, false);
                    else
                        ShowProceduralEat(ti, tj);
                }
                return false;
            }
            return true;
        }


        private void CloseSimpleSteps(List<Button> SimpleStep)
        {
            if (SimpleStep.Count > 0)
            {
                for (int i = 0; i < SimpleStep.Count; ++i)
                {
                    SimpleStep[i].BackColor = GetPreviousButtonBackgroundColor(SimpleStep[i]);
                    SimpleStep[i].Enabled = false;
                }
            }
        }

        private void ShowProceduralEat(int i, int j, bool isOneStep = true)
        {

            int dirX = i - m_PressedButton.Location.Y / m_CellSize,
                dirY = j - m_PressedButton.Location.X / m_CellSize;

            dirX = dirX < 0 ? -1 : 1;
            dirY = dirY < 0 ? -1 : 1;

            int i1 = i,
                j1 = j;

            bool IsEmpty = true;

            while (IsInsideBorder(i1, j1))
            {
                if (m_Map[i1, j1] != 0 && m_Map[i1, j1] != m_CurrentPlayer)
                {
                    IsEmpty = false;
                    break;
                }

                i1 += dirX;
                j1 += dirY;

                if (isOneStep) break;
            }

            //Exit
            if (IsEmpty) return;

            List<Button> toClose = new List<Button>();

            bool CloseSimple = false;

            int ik = i1 + dirX,
                jk = j1 + dirY;

            while (IsInsideBorder(ik, jk))
            {
                if (m_Map[ik, jk] == 0)
                {
                    if (IsButtonHasEatStep(ik, jk, isOneStep, new int[2] { dirX, dirY }))
                    {
                        CloseSimple = true;
                    }
                    else
                    {
                        toClose.Add(m_Buttons[ik, jk]);
                    }
                    m_Buttons[ik, jk].BackColor = Color.Yellow;
                    m_Buttons[ik, jk].Enabled = true;
                    ++m_CountEatSteps;
                }
                else 
                    break;

                if (isOneStep) break;

                ik += dirX;
                jk += dirY;
            }

            if (CloseSimple && toClose.Count > 0)
            {
                CloseSimpleSteps(toClose);
            }
        }

        private bool IsButtonHasEatStep(int iCurrent, int jCurrent, bool isOneStep, int[] direction)
        {
            bool EatStep = false;

            #region 1
            //Вверх по диагонали
            int j = jCurrent + 1;

            //Вниз по диагонали
            for (int i = iCurrent - 1; i >= 0; --i)
            {
                //Не дамка и нет продолжения ходу


                //Не дамка -> только вниз ходят (ограничение по ходу вверх)
                bool flag = true;
                if (direction[0] == 1 && direction[1] == -1 && !isOneStep)
                {  
                    if (i < iCurrent - 2) break;
                    if (i == iCurrent - 1) flag = false;
                }

                if (flag && IsInsideBorder(i, j))
                {
                    if (m_Map[i, j] != 0 && m_Map[i, j] != m_CurrentPlayer)
                    {
                        EatStep = true;

                        if (!IsInsideBorder(i - 1, j + 1))
                            EatStep = false;
                        //Следующая не нулевая -> не можем съесть
                        else if (m_Map[i - 1, j + 1] != 0)
                            EatStep = false;
                        else 
                            return EatStep;

                    }

                    if (j < m_MapSize - 1) ++j;
                    else break;

                    if (isOneStep) break;
                }
            }
            #endregion

            #region 2
            j = jCurrent - 1;

            for (int i = iCurrent - 1; i >= 0; --i)
            {
                //Не дамка -> только вниз ходят (ограничение по ходу вверх)
                bool flag = true;
                if (direction[0] == 1 && direction[1] == 1 && !isOneStep) 
                {
                    if (i < iCurrent - 2) break;
                    if (i == iCurrent - 1) flag = false;
                }
                if (flag && IsInsideBorder(i, j))
                {
                    if (m_Map[i, j] != 0 && m_Map[i, j] != m_CurrentPlayer)
                    {
                        EatStep = true;

                        if (!IsInsideBorder(i - 1, j - 1))
                            EatStep = false;
                        //Следующая не нулевая -> не можем съесть
                        else if (m_Map[i - 1, j - 1] != 0)
                            EatStep = false;
                        else return EatStep;

                    }

                    if (j > 0) --j;
                    else break;

                    if (isOneStep) break;
                }
            }
            #endregion

            #region 3
            j = jCurrent - 1;

            for (int i = iCurrent + 1; i < m_MapSize; ++i)
            {
                //Не дамка -> только вниз ходят (ограничение по ходу вверх)
                bool flag = true;
                if (direction[0] == -1 && direction[1] == 1 && !isOneStep) 
                {
                    if (i > iCurrent + 2) break;
                    if (i == iCurrent + 2) flag = false;
                }

                if (flag && IsInsideBorder(i, j))
                {
                    if (m_Map[i, j] != 0 && m_Map[i, j] != m_CurrentPlayer)
                    {
                        EatStep = true;

                        if (!IsInsideBorder(i + 1, j - 1))
                            EatStep = false;
                        //Следующая не нулевая -> не можем съесть
                        else if (m_Map[i + 1, j - 1] != 0)
                            EatStep = false;
                        else return EatStep;

                    }

                    if (j > 0) --j;
                    else break;

                    if (isOneStep) break;
                }
            }
            #endregion

            #region 4
            j = jCurrent + 1;

            for (int i = iCurrent + 1; i < m_MapSize; ++i)
            {

                //Не дамка -> только вниз ходят (ограничение по ходу вверх)
                bool flag = true;
                if (direction[0] == -1 && direction[1] == -1 && !isOneStep)
                {
                    if (i > iCurrent + 2) break;
                    if (i == iCurrent + 2) flag = false;
                }

                if (flag && IsInsideBorder(i, j))
                {
                    if (m_Map[i, j] != 0 && m_Map[i, j] != m_CurrentPlayer)
                    {
                        EatStep = true;

                        if (!IsInsideBorder(i + 1, j + 1))
                            EatStep = false;
                        //Следующая не нулевая -> не можем съесть
                        else if (m_Map[i + 1, j + 1] != 0)
                            EatStep = false;
                        else 
                            return EatStep;
                    }

                    if (j < m_MapSize - 1) ++j;
                    else 
                        break;

                    if (isOneStep) break;
                }
            }
            #endregion

            return EatStep;
        }

        private void CloseSteps()
        {
            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    m_Buttons[i, j].BackColor = GetPreviousButtonBackgroundColor(m_Buttons[i, j]);
                }
            }
        }

        private bool IsInsideBorder(int i, int j)
        {
            if (i >= m_MapSize || j >= m_MapSize || i < 0 || j < 0) return false;

            return true;
        }

        private void DeactivateAllButtons()
        {
            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    m_Buttons[i, j].Enabled = false;
                }
            }
        }

        private void ActivateAllButtons()
        {
            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    m_Buttons[i, j].Enabled = true;
                }
            }
        }
    
    }
}