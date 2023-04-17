using Explorer700Library;
using System.Drawing;
using TicTacToe.Enums;
using TicTacToe.Extensions;
using TicTacToe.Model;

namespace TicTacToe.Service
{
    public class DrawingService
    {
        public const int NumberOfFields = 3;
        private const int DisplayWidth = 128;
        private const int DisplayHeight = 64;
        private const int DesiredWidth = 64; // Change this value to the desired width which the game should have
        private const int DesiredHeight = 64; // Change this value to the desired height which the game should have
        private const int PenWidth = 1;
        private const int NumberOfLines = NumberOfFields - 1;
        private const int ShapePadding = 4;

        private static readonly int FieldWidth = (int)Math.Floor((float)(DesiredWidth - NumberOfLines * PenWidth) / NumberOfFields);
        private static readonly int FieldHeight = (int)Math.Floor((float)(DesiredHeight - NumberOfLines * PenWidth) / NumberOfFields);

        private static readonly int EffectiveWidth = FieldWidth * NumberOfFields + NumberOfLines * PenWidth;
        private static readonly int EffectiveHeight = FieldHeight * NumberOfFields + NumberOfLines * PenWidth;

        private static readonly int PaddingLeftRight = (DesiredWidth - EffectiveWidth) / 2;
        private static readonly int PaddingTopBottom = (DesiredHeight - EffectiveHeight) / 2;

        private static readonly int ShapeWidth = FieldWidth - ShapePadding * 2;
        private static readonly int ShapeHeight = FieldHeight - ShapePadding * 2;

        private readonly Explorer700 explorer;
        private readonly Graphics graphics;
        private readonly Pen pen;

        public DrawingService(Explorer700 explorer)
        {
            this.explorer = explorer;
            this.graphics = explorer.Display.Graphics;
            this.pen = new Pen(Brushes.Red, PenWidth);
        }

        public void DrawCurrentState(Shape[,] shapes, FieldCoordinate currentPosition)
        {
            this.explorer.Display.Clear();
            this.DrawInitialDisplay();
            this.DrawRectangle(currentPosition.X, currentPosition.Y);

            for (int i = 0; i < shapes.GetLength(0); i++)
            {
                for (int j = 0; j < shapes.GetLength(1); j++)
                {
                    this.Draw(shapes[i, j], i, j);
                }
            }
        }

        public void DrawWinningLine(int xField1, int yField1, int xField2, int yField2)
        {
            var pos1 = this.CalculateTopLeftPointOfField(xField1, yField1);
            var pos2 = this.CalculateTopLeftPointOfField(xField2, yField2);

            pos1.X += ShapeWidth / 2;
            pos1.Y += ShapeHeight / 2;

            pos2.X += ShapeWidth / 2;
            pos2.Y += ShapeHeight / 2;

            this.graphics.DrawLineShifted(this.pen, pos1.X, pos1.Y, pos2.X, pos2.Y);
            this.explorer.Display.Update();
        }

        private void DrawInitialDisplay()
        {
            // vertical lines
            for (int i = 1; i < NumberOfFields; i++)
            {
                var x = (DisplayWidth - DesiredWidth) / 2 + PaddingLeftRight + i * FieldWidth + i * PenWidth;
                var y1 = (DisplayHeight - DesiredHeight) / 2 + PaddingTopBottom;
                var y2 = (DisplayHeight - DesiredHeight) / 2 + PaddingTopBottom + EffectiveHeight;
                this.graphics.DrawLineShifted(this.pen, x, y1, x, y2);
            }

            // horizontal lines
            for (int i = 1; i < NumberOfFields; i++)
            {
                var x1 = (DisplayWidth - DesiredWidth) / 2 + PaddingLeftRight;
                var x2 = (DisplayWidth - DesiredWidth) / 2 + PaddingLeftRight + EffectiveWidth;
                var y = (DisplayHeight - DesiredHeight) / 2 + PaddingTopBottom + i * FieldHeight + i * PenWidth;
                this.graphics.DrawLineShifted(this.pen, x1, y, x2, y);
            }

            this.explorer.Display.Update();
        }

        private void Draw(Shape shape, int xField, int yField)
        {
            if (shape == Shape.Cross)
            {
                this.DrawCross(xField, yField);
            }
            else if (shape == Shape.Ellipse)
            {
                this.DrawEllipse(xField, yField);
            }
        }

        private void DrawEllipse(int xField, int yField)
        {
            this.graphics.DrawEllipseShifted(
                this.pen,
                new Rectangle(
                    this.CalculateTopLeftPointOfField(xField, yField),
                    new Size(ShapeWidth, ShapeHeight)
                    )
                );

            this.explorer.Display.Update();
        }

        private void DrawCross(int xField, int yField)
        {
            var point = this.CalculateTopLeftPointOfField(xField, yField);
            this.graphics.DrawLineShifted(this.pen, point.X, point.Y, point.X + ShapeWidth, point.Y + ShapeHeight);
            this.graphics.DrawLineShifted(this.pen, point.X + ShapeWidth, point.Y, point.X, point.Y + ShapeHeight);

            this.explorer.Display.Update();
        }

        private void DrawRectangle(int xField, int yField)
        {
            var point = this.CalculateTopLeftPointOfField(xField, yField);
            this.graphics.DrawRectangleShifted(this.pen, point.X, point.Y, ShapeWidth, ShapeHeight);

            this.explorer.Display.Update();
        }

        private Point CalculateTopLeftPointOfField(int xField, int yField)
        {
            var x = (DisplayWidth - DesiredWidth) / 2 + PaddingLeftRight + xField * (FieldWidth + PenWidth);
            var y = (DisplayHeight - DesiredHeight) / 2 + PaddingTopBottom + yField * (FieldHeight + PenWidth);
            return new Point(x + ShapePadding, y + ShapePadding);
        }
    }
}
