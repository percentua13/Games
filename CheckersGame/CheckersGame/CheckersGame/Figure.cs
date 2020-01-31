using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame
{
    class Figure
    {
        public readonly Image m_WhiteFigure = new Bitmap(new Bitmap(@"Images\FigureWhite.png"), new Size(Map.m_CellSize - 10, Map.m_CellSize - 10));
        public readonly Image m_BlackFigure = new Bitmap(new Bitmap(@"Images\FigureBlack.png"), new Size(Map.m_CellSize - 10, Map.m_CellSize - 10));
        public readonly Image m_WhiteQueenFigure = new Bitmap(new Bitmap(@"Images\FigureWhiteQueen.png"), new Size(Map.m_CellSize - 10, Map.m_CellSize - 10));
        public readonly Image m_BlackQueenFigure = new Bitmap(new Bitmap(@"Images\FigureBlackQueen.png"), new Size(Map.m_CellSize - 10, Map.m_CellSize - 10));
        public readonly Color m_White = Color.White; //Color.FromArgb(94, 26, 23);
        public readonly Color m_Black = Color.Gray; // Color.FromArgb(234, 205, 167);
    }
}
