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
        public readonly Image m_WhiteFigure = new Bitmap(new Bitmap(@"Images\FigureWhite.png"), new Size(Map.m_CellSize - 12, Map.m_CellSize - 12));
        public readonly Image m_BlackFigure = new Bitmap(new Bitmap(@"Images\FigureBlack.png"), new Size(Map.m_CellSize - 12, Map.m_CellSize - 12));
        public readonly Image m_WhiteQueenFigure = new Bitmap(new Bitmap(@"Images\FigureWhiteQueen.png"), new Size(Map.m_CellSize - 12, Map.m_CellSize - 12));
        public readonly Image m_BlackQueenFigure = new Bitmap(new Bitmap(@"Images\FigureBlackQueen.png"), new Size(Map.m_CellSize - 12, Map.m_CellSize - 12));
        public readonly Color m_White = Color.FromArgb(32, 55, 76);
        public readonly Color m_Black = Color.White;
    }
}
