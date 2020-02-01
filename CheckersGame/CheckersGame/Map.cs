using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersGame
{
    class Map
    {
        CheckersForm form;

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
        private Label lbl_Info;

        int m_CountOfEatenFirstCheckers;
        int m_CountOfEatenSecondCheckers;
        public Map(CheckersForm form)
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
            m_CountOfEatenFirstCheckers = m_CountOfEatenSecondCheckers = 0;

            this.form = form;
            lbl_Info = form.lbl_Info;

            lbl_Info.Text = $"1 gamer : {m_CountOfEatenSecondCheckers}              2 gamer : { m_CountOfEatenFirstCheckers}";
            lbl_Info.Font = new Font("MV Boli", 15);
            lbl_Info.AutoSize = true;
            lbl_Info.Location = new Point(0, Map.m_CellSize * Map.m_MapSize + 5);

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

                    m_Buttons[i, j].FlatAppearance.BorderSize = 0;
                    m_Buttons[i, j].FlatStyle = FlatStyle.Flat;

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
        private void SwitchPlayer(CheckersForm form)
        {
            m_CurrentPlayer = m_CurrentPlayer == 1 ? 2 : 1;
            ResetGame();
        }


        //Check if smb win
        public void ResetGame()
        {
            bool player1 = false;
            bool player2 = false;

            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    if (m_Map[i, j] == 1 || m_Map[i, j] == 3)
                        player1 = true;

                    if (m_Map[i, j] == 2 || m_Map[i, j] == 4)
                        player2 = true;
                }
            }

            if (!player1 || !player2)
            {
                if (!player1) MessageBox.Show("Second player won!");
                else MessageBox.Show("First player won!");

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

            //Сheck that the player has chosen his checker
            if ( (m_Map[i_pressed, j_pressed] == m_CurrentPlayer) || (m_Map[i_pressed, j_pressed] - 2 == m_CurrentPlayer) )
            {
                //Deselect All Checkers
                CloseSteps();

                //Selected checkers highlight
                m_PressedButton.BackColor = Color.Red;
                
                
                DeactivateAllButtons();
                m_PressedButton.Enabled = true;
                m_CountEatSteps = 0;

                //if Queen
                if (m_Map[i_pressed, j_pressed] > 2)
                     ShowSteps(i_pressed, j_pressed, false);
                else
                    ShowSteps(i_pressed, j_pressed);

                if (m_IsMoving)
                {
                    CloseSteps();
                    //m_PressedButton.BackColor = GetPreviousButtonBackgroundColor(m_PressedButton);
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
                          
                    if (m_CountEatSteps >= 1)
                    //if (Math.Abs(j_pressed - j_previous) > 1)
                    {
                        m_IsContinue = true;
                        DeleteEatenCheckers(m_PressedButton, m_PreviousButton);
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

                    if (m_Map[i_pressed, j_pressed] > 2)
                        // if (m_PressedButton.Text == "D")
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

        //Backlight removal
        private Color GetPreviousButtonBackgroundColor(Button m_PreviousButton)
        {
            int i = m_PreviousButton.Location.Y / m_CellSize,
                j = m_PreviousButton.Location.X / m_CellSize;

            if ((i % 2 != 0 && j % 2 == 0) || (i % 2 == 0 && j % 2 != 0))
                return (new Figure()).m_White;
            else
                return (new Figure()).m_Black;
        }

        //Highlight all posible steps
        private void ShowPossibleSteps()
        {
            bool IsOneStep = true;
            bool IsEatStep = false;

            DeactivateAllButtons();

            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    //Allow only current player's checkers
                    if ((m_Map[i, j] == m_CurrentPlayer) || (m_Map[i, j] - 2 == m_CurrentPlayer))
                    {
                        
                        IsOneStep = (m_Map[i, j] > 2) ? false : true;

                        //Enable all checkers that have "eaten steps"
                        if (IsButtonHasEatStep(i, j, IsOneStep, new int[2] { 0, 0 }))
                        {
                            IsEatStep = true;
                            m_Buttons[i, j].Enabled = true;
                        }
                    }
                }
            }
            //If there are no "eaten steps" => activate all checkers
            if (!IsEatStep) ActivateAllButtons();
        }

        //Check if the current checker is queen
        private void SwitchButtonToCheat(Button button)
        {
            int i = button.Location.Y / m_CellSize,
                j = button.Location.X / m_CellSize;

            if (m_Map[i,j] == 1 && i == m_MapSize - 1)
            {
                m_Map[i, j] = 3;
                button.Image = (new Figure()).m_WhiteQueenFigure;
                //button.Text = "D";
            }

            if (m_Map[i, j] == 2 && i == 0)
            {
                m_Map[i, j] = 4;
                button.Image = (new Figure()).m_BlackQueenFigure;
                //button.Text = "D";
            }
        }
         
        //Delete all eaten checkers
        private void DeleteEatenCheckers(Button EndButton, Button StartButton)
        {
            int Count = Math.Abs(EndButton.Location.Y / m_CellSize - StartButton.Location.Y / m_CellSize);

            int StartX = EndButton.Location.Y / m_CellSize - StartButton.Location.Y / m_CellSize,
                StartY = EndButton.Location.X / m_CellSize - StartButton.Location.X / m_CellSize;

            StartX = StartX < 0 ? -1 : 1;
            StartY = StartY < 0 ? -1 : 1;

            int CurrentCount = 0;

            int i = StartButton.Location.Y / m_CellSize + StartX, 
                j = StartButton.Location.X / m_CellSize + StartY; 

            //Remove all eaten checkers
            while (CurrentCount < Count - 1)
            {
                if (m_CurrentPlayer == 1 && m_Map[i, j] != 0) ++m_CountOfEatenSecondCheckers;
                if (m_CurrentPlayer == 2 && m_Map[i, j] != 0) ++m_CountOfEatenFirstCheckers;

                m_Map[i, j] = 0;
                m_Buttons[i, j].Image = null;
                m_Buttons[i, j].Text = "";

                i += StartX;
                j += StartY;

                ++CurrentCount;

                lbl_Info.Text = $"1 gamer : {m_CountOfEatenSecondCheckers}              2 gamer : { m_CountOfEatenFirstCheckers}";
            }

        }

        private void ShowSteps(int iCurrent, int jCurrent, bool IsOneStep = true)
        {
            //Clear list of old simple steps
            m_SimpleSteps.Clear();

            //Show all possible steps
            ShowDiagonal(iCurrent, jCurrent, IsOneStep);

            //If there is at least one "eaten step" => disable all simple steps
            if (m_CountEatSteps > 0)
                CloseSimpleSteps(m_SimpleSteps);
        }


        private void ShowDiagonal(int iCurrent, int jCurrent, bool IsOneStep = false)
        {
            //"White" checkers can walk only down (but allow up eaten step) - 1 long step allow
            //"Black" checkers - only up (but allow down eaten step) - 1 long step allo
            //Queens can go up and down, there is no length limit 

            #region 1 Direction: up and right

            int j = jCurrent + 1;

            for (int i = iCurrent - 1; i >= 0; --i)
            {
                if (m_CurrentPlayer == 1)
                {
                    //Back step : restrict simple back step but allow "eaten back step"
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, true, m_Map[iCurrent, jCurrent] > 2)) break;
                }
                else
                {
                    //Forward step
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, false, m_Map[iCurrent, jCurrent] > 2)) break;
                }

                //Move to the next cell (if it's not end of board)
                if (j < m_MapSize - 1) ++j;
                else 
                    break;

                //If it isn't a queen => allow only 1 long step
                if (IsOneStep) break;
            }
            #endregion

            #region 2 Direction: up and left
            j = jCurrent - 1;

            for (int i = iCurrent - 1; i >= 0; --i)
            {

                if (m_CurrentPlayer == 1)
                {
                    //Back step : restrict simple back step but allow "eaten back step"
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, true, m_Map[iCurrent, jCurrent] > 2)) break;
                }
                else
                {
                    //Forward step
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, false, m_Map[iCurrent, jCurrent] > 2)) break;
                }

                //Move to the next cell (if it's not end of board)
                if (j > 0) --j;
                else break;

                //If it isn't a queen => allow only 1 long step
                if (IsOneStep) break;
            }
            #endregion

            #region 3 Direction : down and right
            j = jCurrent + 1;

            for (int i = iCurrent + 1; i < m_MapSize; ++i)
            {

                if (m_CurrentPlayer == 2)
                {
                    //Back step : restrict simple back step but allow "eaten back step"
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, true, m_Map[iCurrent, jCurrent] > 2)) break;
                }
                else
                {
                    //Forward step
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, false, m_Map[iCurrent, jCurrent] > 2)) break;
                }

                //Move to the next cell (if it's not end of board)
                if (j < m_MapSize - 1) ++j;
                else
                    break;

                //If it isn't a queen => allow only 1 long step
                if (IsOneStep) break;
            }
            #endregion

            #region 4 Direction : down and left
            j = jCurrent - 1;

            for (int i = iCurrent + 1; i < m_MapSize; ++i)
            {

                if (m_CurrentPlayer == 2)
                {
                    //Back step : restrict simple back step but allow "eaten back step"
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, true, m_Map[iCurrent, jCurrent] > 2)) break;
                }
                else
                {
                    //Forward step
                    if (IsInsideBorder(i, j) && !DeterminePath(i, j, false, m_Map[iCurrent, jCurrent] > 2)) break;
                }

                //Move to the next cell (if it's not end of board)
                if (j > 0) --j;
                else 
                    break;

                //If it isn't a queen => allow only 1 long step
                if (IsOneStep) break;
            }
            #endregion           
        }


        //If there is an avaliable step
        private bool DeterminePath(int ti, int tj, bool BiteBack, bool IsQueen)
        {
            //Restrict simple back step (allow "eaten back step")

            //Simple step
            if ( (IsQueen || !BiteBack) && m_Map[ti, tj] == 0 && !m_IsContinue)
            {
                m_Buttons[ti, tj].BackColor = Color.Yellow;
                m_Buttons[ti, tj].Enabled = true;
                m_SimpleSteps.Add(m_Buttons[ti, tj]);
            }
            //Eaten step
            else if (BiteBack || (!BiteBack && m_Map[ti, tj] != 0))
            {
                if (m_Map[ti, tj] != m_CurrentPlayer && m_Map[ti, tj] - 2 != m_CurrentPlayer)
                {
                    if (m_Map[m_PressedButton.Location.Y/m_CellSize, m_PressedButton.Location.X / m_CellSize] > 2)
                         ShowProceduralEat(ti, tj, false);
                    else
                        ShowProceduralEat(ti, tj);
                }
                return false;
            }
            return true;
        }

        //if there is at least one "eaten step" => close all simple("not eaten") steps
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

        //Highlight all posible steps
        private void ShowProceduralEat(int i, int j, bool isOneStep = true)
        {

            int dirX = i - m_PressedButton.Location.Y / m_CellSize, 
                dirY = j - m_PressedButton.Location.X / m_CellSize;  

            dirX = dirX < 0 ? -1 : 1;
            dirY = dirY < 0 ? -1 : 1;

            int i1 = i,
                j1 = j;

            bool IsEmpty = true;

            //Check if there is an "eaten step"
            while (IsInsideBorder(i1, j1))
            {
                if (m_Map[i1, j1] != 0 && m_Map[i1, j1] != m_CurrentPlayer)
                {
                    IsEmpty = false;
                    break;
                }

                i1 += dirX;
                j1 += dirY;

                //If isn't queen => allow 1 long step
                if (isOneStep) break;
            }

            //No "eaten" steps
            if (IsEmpty) return;

            //All "simple steps" that need to be disabled
            List<Button> toClose = new List<Button>();

            bool CloseSimple = false;

            int ik = i1 + dirX,
                jk = j1 + dirY;

            bool CloseSimpleForCurrentChecker = true;

            while (IsInsideBorder(ik, jk))
            {
                
                //Highlight only empty cells
                if (m_Map[ik, jk] == 0)
                {
                    //If we have at least one "eaten step" => all simple steps need to be disabled
                    if (IsButtonHasEatStep(ik, jk, isOneStep, new int[2] { dirX, dirY }))
                    {
                        CloseSimple = true;
                        CloseSimpleForCurrentChecker = false;
                    }
                    //Simple step
                    else if (CloseSimpleForCurrentChecker)
                    {
                        toClose.Add(m_Buttons[ik, jk]);
                    }
                    
                    m_Buttons[ik, jk].BackColor = Color.Yellow;
                    m_Buttons[ik, jk].Enabled = true;
                    ++m_CountEatSteps;
                }
                else 
                    break;

                //If isn't queen => allow 1 long step
                if (isOneStep) break;

                ik += dirX;
                jk += dirY;
            }

            //If there are "eaten steps" => delete all "simple steps"
            if (CloseSimple && toClose.Count > 0)
            {
                CloseSimpleSteps(toClose);
            }
        }

        private bool IsButtonHasEatStep(int iCurrent, int jCurrent, bool isOneStep, int[] direction)
        {
            bool EatStep = false;

            #region 1 Direction: up and right


            int j = jCurrent + 1;

            for (int i = iCurrent - 1; i >= 0; --i)
            {
                bool CanCreateStep = true;

                //Allow step back for current checker (to check if there is an "eaten step")                              
                if (direction[0] == 1 && direction[1] == -1 && !isOneStep)
                {  
                    if (i < iCurrent - 2) break;
                    if (i == iCurrent - 1) CanCreateStep = false;
                }

                if (CanCreateStep && IsInsideBorder(i, j))
                {
                    if (m_Map[i, j] != 0 && (m_Map[i, j] != m_CurrentPlayer && m_Map[i, j] - 2 != m_CurrentPlayer))
                    {
                        EatStep = true;

                        //The next isn't at board
                        if (!IsInsideBorder(i - 1, j + 1))
                        {
                            EatStep = false;
                            break;
                        }
                        //The next is not empty -> can't eat
                        else if (m_Map[i - 1, j + 1] != 0)
                        {
                            EatStep = false;
                            break;
                        }
                        else
                            return EatStep;
                    }

                    //Move to the next cell (if it's not end of board)
                    if (j < m_MapSize - 1) ++j;
                    else 
                        break;

                    //If it isn't a queen => allow only 1 long step
                    if (isOneStep) break;
                }
            }
            #endregion

            #region 2 Direction: up and left

            j = jCurrent - 1;

            for (int i = iCurrent - 1; i >= 0; --i)
            {
                bool CanCreateStep = true;

                //Allow step back for current checker (to check if there is an "eaten step") 
                if (direction[0] == 1 && direction[1] == 1 && !isOneStep) 
                {
                    if (i < iCurrent - 2) break;
                    if (i == iCurrent - 1) CanCreateStep = false;
                }
                if (CanCreateStep && IsInsideBorder(i, j))
                {
                    if (m_Map[i, j] != 0 && (m_Map[i, j] != m_CurrentPlayer && m_Map[i, j] - 2 != m_CurrentPlayer))
                    {
                        EatStep = true;

                        //The next isn't at board
                        if (!IsInsideBorder(i - 1, j - 1))
                        {
                            EatStep = false;
                            break;
                        }
                        //The next is not empty -> can't eat
                        else if (m_Map[i - 1, j - 1] != 0)
                        {
                            EatStep = false;
                            break;
                        }
                        else return EatStep;

                    }

                    //Move to the next cell (if it's not end of board)
                    if (j > 0) --j;
                    else break;

                    //If it isn't a queen => allow only 1 long step
                    if (isOneStep) break;
                }
            }
            #endregion

            #region 3 Direction: down and right
            j = jCurrent + 1;

            for (int i = iCurrent + 1; i < m_MapSize; ++i)
            {
                bool CanCreateStep = true;

                //Allow step back for current checker (to check if there is an "eaten step") 
                if (direction[0] == -1 && direction[1] == -1 && !isOneStep)
                {
                    if (i > iCurrent + 2) break;
                    if (i == iCurrent + 2) CanCreateStep = false;
                }

                if (CanCreateStep && IsInsideBorder(i, j))
                {
                    if (m_Map[i, j] != 0 && (m_Map[i, j] != m_CurrentPlayer && m_Map[i, j] - 2 != m_CurrentPlayer))
                    {
                        EatStep = true;

                        //The next isn't at board
                        if (!IsInsideBorder(i + 1, j + 1))
                        {
                            EatStep = false;
                            break;
                        }
                        //The next is not empty -> can't eat
                        else if (m_Map[i + 1, j + 1] != 0)
                        {
                            EatStep = false;
                            break;
                        }
                        else
                            return EatStep;
                    }

                    //Move to the next cell (if it's not end of board)
                    if (j < m_MapSize - 1) ++j;
                    else
                        break;

                    //If it isn't a queen => allow only 1 long step
                    if (isOneStep) break;
                }
            }
            #endregion

            #region 4 Direction: down and left
            j = jCurrent - 1;

            for (int i = iCurrent + 1; i < m_MapSize; ++i)
            {              
                bool CanCreateStep = true;

                //Allow step back for current checker (to check if there is an "eaten step") 
                if (direction[0] == -1 && direction[1] == 1 && !isOneStep) 
                {
                    if (i > iCurrent + 2) break;
                    if (i == iCurrent + 2) CanCreateStep = false;
                }

                if (CanCreateStep && IsInsideBorder(i, j))
                {
                    if (m_Map[i, j] != 0 && (m_Map[i, j] != m_CurrentPlayer && m_Map[i, j] - 2 != m_CurrentPlayer))
                    {
                        EatStep = true;

                        //The next isn't at board
                        if (!IsInsideBorder(i + 1, j - 1))
                        {
                            EatStep = false;
                            break;
                        }
                        //The next is not empty => can't eat
                        else if (m_Map[i + 1, j - 1] != 0)
                        {
                            EatStep = false;
                            break;
                        }
                        else return EatStep;
                    }
                    //Move to the next cell (if it's not end of board)
                    if (j > 0) --j;
                    else 
                        break;

                    //If it isn't a queen => allow only 1 long step
                    if (isOneStep) break;
                }
            }
            #endregion

            return EatStep;
        }

        //Deselect All Checkers (Set property Background)
        private void CloseSteps()
        {
            #region
            for (int i = 0; i < m_MapSize; ++i)
            {
                for (int j = 0; j < m_MapSize; ++j)
                {
                    m_Buttons[i, j].BackColor = GetPreviousButtonBackgroundColor(m_Buttons[i, j]);
                }
            }
            #endregion
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