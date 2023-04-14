using Explorer700Library;
using TicTacToe.Enums;
using System.Drawing;

namespace TicTacToe.Service
{
    public class TicTacToeService
    {
        private readonly DrawingService drawingService;
        private readonly BuzzerService buzzerService;
        private readonly Explorer700 explorer;
        private readonly Shape[,] shapes;
        private int currentPosition = 1;
        private Shape currentPlayer = Shape.Cross;

        public TicTacToeService(Explorer700 explorer)
        {
            this.explorer = explorer;
            this.drawingService = new DrawingService(this.explorer);
            this.buzzerService = new BuzzerService(this.explorer);
            this.shapes = new Shape[DrawingService.NumberOfFields, DrawingService.NumberOfFields];
        }

        public void StartGame()
        {
            var position = this.GetCurrentPosition();
            this.drawingService.DrawCurrentState(this.shapes, position);
            this.explorer.Joystick.JoystickChanged += this.JoysticChanged;
        }

        private void JoysticChanged(object? sender, KeyEventArgs e)
        {
            if (e.Keys == Keys.NoKey)
            {
                return;
            }

            if (e.Keys == Keys.Center)
            {
                var position = this.GetCurrentPosition();
                if (this.shapes[position.X, position.Y] != Shape.None)
                {
                    return;
                }

                this.shapes[position.X, position.Y] = this.currentPlayer;
                this.drawingService.DrawCurrentState(this.shapes, this.GetCurrentPosition());
                if (this.CheckGameStatus() != Shape.None)
                {
                    this.drawingService.DrawWinningLine(0, 0, 2, 2);
                    this.buzzerService.ItsBuzzinTime();
                }

                this.currentPlayer = this.currentPlayer == Shape.Cross ? Shape.Ellipse : Shape.Cross;
            }
            else if (e.Keys == Keys.Right)
            {
                if (this.currentPosition == DrawingService.NumberOfFields * DrawingService.NumberOfFields)
                {
                    this.currentPosition = 1;
                }
                else
                {
                    this.currentPosition++;
                }
            }
            else if (e.Keys == Keys.Left)
            {
                if (this.currentPosition == 1)
                {
                    this.currentPosition = DrawingService.NumberOfFields * DrawingService.NumberOfFields;
                }
                else
                {
                    this.currentPosition--;
                }
            }
            else if (e.Keys == Keys.Up)
            {
                if (this.currentPosition <= DrawingService.NumberOfFields)
                {
                    this.currentPosition += DrawingService.NumberOfFields * (DrawingService.NumberOfFields - 1);
                }
                else
                {
                    this.currentPosition -= DrawingService.NumberOfFields;
                }
            }
            else if (e.Keys == Keys.Down)
            {
                if (this.currentPosition > DrawingService.NumberOfFields * (DrawingService.NumberOfFields - 1))
                {
                    this.currentPosition -= DrawingService.NumberOfFields * (DrawingService.NumberOfFields - 1);
                }
                else
                {
                    this.currentPosition += DrawingService.NumberOfFields;
                }
            }

            if (e.Keys != Keys.Center)
            {
                this.drawingService.DrawCurrentState(this.shapes, this.GetCurrentPosition());
            }
        }

        private Point GetCurrentPosition()
        {
            var x = (this.currentPosition - 1) % DrawingService.NumberOfFields;
            var y = (int)Math.Ceiling((double)this.currentPosition / DrawingService.NumberOfFields) - 1;

            return new Point(x, y);
        }

        private Shape CheckGameStatus()
        {
            var shape = this.CheckVertical();
            if (shape != Shape.None)
            {
                return shape;
            }

            shape = this.CheckHorizontal();
            if (shape != Shape.None)
            {
                return shape;
            }

            shape = this.CheckDiagonal1();
            if (shape != Shape.None)
            {
                return shape;
            }

            shape = this.CheckDiagonal2();
            return shape;
        }


        private Shape CheckVertical()
        {
            for (int i = 0; i < DrawingService.NumberOfFields; i++)
            {
                var currentShape = this.shapes[i, 0];

                if (currentShape == Shape.None)
                {
                    continue;
                }

                for (int j = 1; j < DrawingService.NumberOfFields; j++)
                {
                    if (currentShape != this.shapes[i, j])
                    {
                        break;
                    }

                    if (j == DrawingService.NumberOfFields - 1)
                    {
                        return currentShape;
                    }
                }
            }

            return Shape.None;
        }

        private Shape CheckHorizontal()
        {
            for (int i = 0; i < DrawingService.NumberOfFields; i++)
            {
                var currentShape = this.shapes[0, i];

                if (currentShape == Shape.None)
                {
                    continue;
                }

                for (int j = 1; j < DrawingService.NumberOfFields; j++)
                {
                    if (currentShape != this.shapes[j, i])
                    {
                        break;
                    }

                    if (j == DrawingService.NumberOfFields - 1)
                    {
                        return currentShape;
                    }
                }
            }

            return Shape.None;
        }

        private Shape CheckDiagonal1()
        {
            var currentShape = this.shapes[0, 0];
            for (int i = 1; i < DrawingService.NumberOfFields; i++)
            {
                if (currentShape == Shape.None || currentShape != this.shapes[i, i])
                {
                    break;
                }

                if (i == DrawingService.NumberOfFields - 1)
                {
                    return currentShape;
                }
            }

            return Shape.None;
        }

        private Shape CheckDiagonal2()
        {
            var currentShape = this.shapes[0, DrawingService.NumberOfFields - 1];
            for (int i = 1; i < DrawingService.NumberOfFields; i++)
            {
                if (currentShape == Shape.None || currentShape != this.shapes[i, DrawingService.NumberOfFields - 1 - i])
                {
                    break;
                }

                if (i == DrawingService.NumberOfFields - 1)
                {
                    return currentShape;
                }
            }

            return Shape.None;
        }
    }
}
