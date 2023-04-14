using System.Drawing;

namespace TicTacToe.Extensions
{
    public static class GraphicsExtensions
    {
        public static void DrawLineShifted(this Graphics graphics, Pen pen, int x1, int y1, int x2, int y2)
        {
            graphics.DrawLine(pen, x1 - 1, y1 - 1, x2 - 1, y2 - 1);
        }

        public static void DrawEllipseShifted(this Graphics graphics, Pen pen, Rectangle rectangle)
        {
            rectangle.X--;
            rectangle.Y--;
            graphics.DrawEllipse(pen, rectangle);
        }

        public static void DrawRectangleShifted(this Graphics graphics, Pen pen, int x, int y, int width, int height)
        {
            graphics.DrawRectangle(pen, x - 1, y - 1, width, height);
        }
    }
}
