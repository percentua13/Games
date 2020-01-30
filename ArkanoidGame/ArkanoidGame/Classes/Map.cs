using ArkanoidGame.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace ArkanoidGame
{
    public class Map : IMap
    {
        #region fields
        //Determines how much score needs to be (KoefToAddLine*COUNT_OF_BROKEN_PLATFORMS_TO_ADD_LINES) to add new line;
        private int m_KoefToAddLine = 1;

        private IBall m_GameBall;

        internal const int m_MapWidth = 20;
        internal const int m_MapHeight = 30;
        internal const int m_PixelSize = 20;

        private int[,] m_Map;

        private Platform m_ArkanoidPlatform;

        private Set m_ArkanoidSet;
        #endregion
        
       public Map()
       {
            #region
            m_Map = new int[m_MapHeight + 2, m_MapWidth + 2];

            m_GameBall = new Ball();

            m_ArkanoidSet = new Set();

            m_ArkanoidPlatform = new Platform();
            #endregion
       }

        //Clear the game field
        private void ClearGame()
        {
            #region
            for (int i = 0; i < m_MapHeight; ++i)
            {
                for (int j = 0; j < m_MapWidth; ++j)
                {
                    m_Map[i, j] = 0;
                }
            }
            #endregion
        }

        //Set initial properties to start the game
        public void SetInitialProperities()
        {
            #region

            m_KoefToAddLine = 1;

            //Initial count of lines 
            m_GameBall.m_MaxPlatformY = m_GameBall.m_MaxPlatformY;

            //Initial Coordinates of Set - (center - bottom of map) 
            m_ArkanoidSet.m_iSetX = (m_MapWidth-3) / 2;
            m_ArkanoidSet.m_iSetY = m_MapHeight - 1;

            ClearGame();
            GeneratePlatforms();

            m_GameBall.SetInitProperities(m_ArkanoidSet);

            m_Map[m_ArkanoidSet.m_iSetY, m_ArkanoidSet.m_iSetX] = (int)ENUM_Properities.SET1;
            m_Map[m_ArkanoidSet.m_iSetY, m_ArkanoidSet.m_iSetX + 1] = (int)ENUM_Properities.SET2;
            m_Map[m_ArkanoidSet.m_iSetY, m_ArkanoidSet.m_iSetX + 2] = (int)ENUM_Properities.SET3;
            m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX] = (int)ENUM_Properities.BALL;
            #endregion
        }

        //Choose random color for each platform
        private void GeneratePlatforms()
        {
            #region
            Random rand = new Random();

            for (int i = 0; i < m_GameBall?.m_MaxPlatformY; ++i)
            {
                for (int j = 0; j < m_MapWidth; j += 2)
                {
                    //Create random platform(color)
                    int index = rand.Next(1, Platform.CountOfPlatforms);

                    //Begin of platform will have positive value, end - negative value
                    m_Map[i, j] = index;
                    m_Map[i, j + 1] = - index;
                }
            }
            #endregion
        }

        //Add new line whether is there free space
        private bool CanAddNewLine()
        {
            #region
            //New Game
            if (m_GameBall.m_MaxPlatformY + 1 >= m_MapHeight - 1) return false;


            //Lines shift down for 1 position
            for (int i = m_GameBall.m_MaxPlatformY; i > 0; --i)
            {
                for (int j = 0; j < m_MapWidth; j += 2)
                {
                    if (m_Map[i - 1, j] != (int)ENUM_Properities.BALL && m_Map[i, j] != (int)ENUM_Properities.BALL)
                        m_Map[i, j] = m_Map[i - 1, j];
                }
            }


            //Fill the first line
            Random rand = new Random();
            for (int j = 0; j < m_MapWidth; j += 2)
            {
                if (m_Map[0, j] != (int)ENUM_Properities.BALL && m_Map[0, j + 1] != (int)ENUM_Properities.BALL)
                {
                    m_Map[0, j] = rand.Next(1, Platform.CountOfPlatforms);
                    m_Map[0, j + 1] = - m_Map[0, j];
                }
            }

            //Increase the count of lines
            ++m_GameBall.m_MaxPlatformY;

            return true;
            #endregion
        }

        public void DrawMap(Graphics g)
        {
            #region
            for (int i = 0; i < m_MapHeight; ++i)
            {
                for (int j = 0; j < m_MapWidth; ++j)
                {

                    //If SET (length : 3 px, width : 1 px)
                    if (m_Map[i, j] == (int)ENUM_Properities.SET1)
                    {
                        g.DrawImage(m_ArkanoidSet.SetImage, new Rectangle(new Point((int)ENUM_Properities.LEFT_MARGIN + j * m_PixelSize, i * m_PixelSize), new Size(3 * (int)ENUM_Properities.PIXEL_SIZE, (int)ENUM_Properities.PIXEL_SIZE)), 0, 0, 64, 16, GraphicsUnit.Pixel);
                        continue;
                    }

                    //If BALL (length : 1 px, width : 1 px)
                    if (m_Map[i, j] == (int)ENUM_Properities.BALL)
                    {
                        g.DrawImage(m_GameBall.m_ArkanoidBall, new Rectangle(new Point((int)ENUM_Properities.LEFT_MARGIN + j * m_PixelSize, i * m_PixelSize), new Size((int)ENUM_Properities.PIXEL_SIZE, (int)ENUM_Properities.PIXEL_SIZE)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                        continue;
                    }

                    //If PLATFORM (length : 2 px, width : 1 px)
                    if (m_Map[i, j] > 0 && m_Map[i, j] <= Platform.CountOfPlatforms)
                    {
                        (int X_1, int Y_1, int X_Size, int Y_Size) = m_ArkanoidPlatform[m_Map[i, j] - 1];
                        g.DrawImage(m_ArkanoidPlatform.PlatformImage, new Rectangle(new Point((int)ENUM_Properities.LEFT_MARGIN + j * m_PixelSize, i * m_PixelSize), new Size(2 * (int)ENUM_Properities.PIXEL_SIZE, (int)ENUM_Properities.PIXEL_SIZE)), X_1, Y_1, X_Size, Y_Size, GraphicsUnit.Pixel);
                        continue;
                    }
                }
            }
            #endregion
        }


        /* 
        * Determines if there is an obstacle  : Walls, Platform, Set
        * If true : Walls and Platform => Change directory
        *         : Platform => Crash platform and change directory
        */
        private bool IsCollide(ref int Score)
        {
            #region

            bool IsCollide = false;

            #region WALLS
            //COLLISION WITH WALL
            //Check left and right borders
            if (m_GameBall.m_iBallX + m_GameBall.m_iDirectionX  > m_MapWidth - 1 || m_GameBall.m_iBallX + m_GameBall.m_iDirectionX  < 0)
            {
                m_GameBall.m_iDirectionX *= -1;
                IsCollide = true;
            }

            //COLLISION WITH WALL
            //Check upper and lower borders
            if (m_GameBall.m_iBallY + m_GameBall.m_iDirectionY >= m_MapHeight || m_GameBall.m_iBallY + m_GameBall.m_iDirectionY < 0)
            {
                m_GameBall.m_iDirectionY *= -1;
                IsCollide = true;
            }
            #endregion

            #region SET
            //COLLISION WITH SET
            //Check if set under the ball
            if (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX] >= (int)ENUM_Properities.SET1)
            {
                m_GameBall.m_iDirectionY *= -1;
                IsCollide = true;

                return IsCollide;
            }

            //COLLISION WITH SET
            //Check if set is diagonal to the ball
            if (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] >= (int)ENUM_Properities.SET1)
            {
                m_GameBall.m_iDirectionX *= -1;
                m_GameBall.m_iDirectionY *= -1;
                IsCollide = true;

               return IsCollide;
            }
            #endregion

            #region PLATFORMS
            //COLLISION WITH PLATFORM
            //If ball doesn't cross out the borders => check for collision with platforms
            if (m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] < (int)ENUM_Properities.SET1 && m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] != (int)ENUM_Properities.EMPTY_BLOCK)
            {          
                //if touch to begin of platform => delete end of platform
                if (m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] > 0)
                {
                    //if ball on this part => leave ball else => free space
                    m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX + 1] = (m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX + 1] == (int)ENUM_Properities.BALL) ? (int)ENUM_Properities.BALL : 0;
                }
                //if touch to end of platform => delete begin of platform
                else
                {
                    //if ball on this part => leave ball else => free space
                    m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX - 1] = (m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX - 1] == (int)ENUM_Properities.BALL) ? (int)ENUM_Properities.BALL : 0;
                }

                //delete platform : if ball on this part => leave ball else => free space
                m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] = (m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] == (int)ENUM_Properities.BALL) ? (int)ENUM_Properities.BALL : 0;

                //Change direction
                m_GameBall.m_iDirectionX *= -1;

                ++Score;

                IsCollide = true;

                return IsCollide;
            }

            //COLLISION WITH PLATFORM
            //If ball doesn't cross out the borders => check for collision with platforms
            if (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX] < (int)ENUM_Properities.SET1 && m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX] != (int)ENUM_Properities.EMPTY_BLOCK)
            {
               
                //if touch to begin of platform => delete end of platform
                if (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX] > 0)
                {
                    //if ball on this part => leave ball else => free space
                    m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + 1] = (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + 1] == (int)ENUM_Properities.BALL) ? (int)ENUM_Properities.BALL : 0;

                }
                //if touch to end of platform => delete begin of platform
                else
                {
                    //if ball on this part => leave ball else => free space
                    m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX - 1] = (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX - 1] == (int)ENUM_Properities.BALL) ? (int)ENUM_Properities.BALL : 0;
                }

                //delete platform : if ball on this part => leave ball else => free space
                m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX] = (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX] == (int)ENUM_Properities.BALL) ? (int)ENUM_Properities.BALL : 0;

                //Change direction
                m_GameBall.m_iDirectionY *= -1;

                ++Score;

                IsCollide = true;

                return IsCollide;
            }

            //COLLISION WITH PLATFORM
            //If ball doesn't cross out the borders => check for collision with platforms
            if (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] < (int)ENUM_Properities.SET1 && m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] != (int)ENUM_Properities.EMPTY_BLOCK)
            {
                //if touch to begin of platform => delete end of platform
                if (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] > 0)
                {
                    //if ball on this part => leave ball else => free space
                    m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX + 1] = (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX + 1] == (int)ENUM_Properities.BALL) ? (int)ENUM_Properities.BALL : 0;
                }
                //if touch to end of platform => delete begin of platform
                else
                {
                    //if ball on this part => leave ball else => free space
                    m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX - 1] = (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX - 1] == (int)ENUM_Properities.BALL) ? (int)ENUM_Properities.BALL : 0;
                }


                //delete platform : if ball on this part => leave ball else => free space
                m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] = (m_Map[m_GameBall.m_iBallY + m_GameBall.m_iDirectionY, m_GameBall.m_iBallX + m_GameBall.m_iDirectionX] == (int)ENUM_Properities.BALL) ? (int)ENUM_Properities.BALL : 0;


                //Change direction
                m_GameBall.m_iDirectionX *= -1;
                m_GameBall.m_iDirectionY *= -1;

                ++Score;
                IsCollide = true;

                return IsCollide;
            }
            #endregion

            return IsCollide;
            #endregion
        }


        // Button click processing
        public void KeyInputCheck(object sender, KeyEventArgs e)
        {
            #region
            if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right) return;

            //Set has length = 3 points
            m_Map[m_ArkanoidSet.m_iSetY, m_ArkanoidSet.m_iSetX] = 0;
            m_Map[m_ArkanoidSet.m_iSetY, m_ArkanoidSet.m_iSetX + 1] = 0;
            m_Map[m_ArkanoidSet.m_iSetY, m_ArkanoidSet.m_iSetX + 2] = 0;

            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (m_ArkanoidSet.m_iSetX + 3 < m_MapWidth)
                        ++m_ArkanoidSet.m_iSetX;
                    break;

                case Keys.Left:
                    if (m_ArkanoidSet.m_iSetX - 1 >= 0)
                        --m_ArkanoidSet.m_iSetX;
                    break;
            }

            m_Map[m_ArkanoidSet.m_iSetY, m_ArkanoidSet.m_iSetX] = (int)ENUM_Properities.SET1;
            m_Map[m_ArkanoidSet.m_iSetY, m_ArkanoidSet.m_iSetX + 1] = (int)ENUM_Properities.SET2;
            m_Map[m_ArkanoidSet.m_iSetY, m_ArkanoidSet.m_iSetX + 2] = (int)ENUM_Properities.SET3;
            #endregion
        }


        //Update info when ball moves
        public bool IfMapCanBeApdate(ref int Score)
        {
            #region
            //Win the game
            if (CheckIfWin()) return false;

            //When the ball touched the bottom of the field => New game
            if (m_GameBall.m_iBallY + m_GameBall.m_iDirectionY >= m_MapHeight) return false;


            //Move to the next position
            if (!IsCollide(ref Score))
            {
                m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX] = 0;
                m_GameBall.m_iBallX += m_GameBall.m_iDirectionX;
                m_GameBall.m_iBallY += m_GameBall.m_iDirectionY;
                m_Map[m_GameBall.m_iBallY, m_GameBall.m_iBallX] = (int)ENUM_Properities.BALL;

                if (m_KoefToAddLine * (int)ENUM_Properities.COUNT_OF_BROKEN_PLATFORMS_TO_ADD_LINES <= Score)
                {
                    if (!CanAddNewLine()) return false;

                    ++m_KoefToAddLine;
                }
            }

            return true;
            #endregion
        }

        public bool CheckIfWin()
        {
            #region
            for (int i = 0; i<m_MapHeight; ++i)
            {
                for (int j = 0; j<m_MapWidth; ++j)
                {
                    if (m_Map[i, j] != 0 && m_Map[i, j] <= Platform.CountOfPlatforms) return false;
                }
            }

            return true;
            #endregion
        }
       
    }
}
