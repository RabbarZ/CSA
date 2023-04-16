using Explorer700Library;
using TicTacToe.Enums;
using System.Drawing;
using TicTacToe.Model;

namespace TicTacToe.Service
{
    public class TicTacToeService
    {
        private readonly DrawingService drawingService;
        private readonly BuzzerService buzzerService;
        private readonly Explorer700 explorer;
        private Shape[,]? shapes;
        private int currentPosition;
        private Shape currentPlayer;

        public TicTacToeService(Explorer700 explorer)
        {
            this.explorer = explorer;
            this.drawingService = new DrawingService(this.explorer);
            this.buzzerService = new BuzzerService(this.explorer);
        }

        public void StartGame()
        {
            this.currentPosition = 1;
            this.currentPlayer = Shape.Cross;
            this.shapes = new Shape[DrawingService.NumberOfFields, DrawingService.NumberOfFields];
            var position = this.GetCurrentPosition();
            this.drawingService.DrawCurrentState(this.shapes, position);
            this.explorer.Joystick.JoystickChanged += this.JoysticChanged;
        }

        public void StopGame()
        {
            this.explorer.Joystick.JoystickChanged -= this.JoysticChanged;
        }

        private void RestartGame()
        {
            this.StopGame();
            this.StartGame();
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
                var gameState = this.GetGameState();
                if (gameState.Winner != Shape.None || gameState.Draw)
                {
                    var beepTime = 200;
                    if (!gameState.Draw)
                    {
                        this.drawingService.DrawWinningLine(gameState.WinningStartField.X, gameState.WinningStartField.Y, gameState.WinningEndField.X, gameState.WinningEndField.Y);
                        beepTime = 1000;
                    }

                    this.buzzerService.ItsBuzzinTime(beepTime);
                    Task.Delay(TimeSpan.FromSeconds(3)).Wait();
                    this.RestartGame();
                    return;
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

        private GameState GetGameState()
        {
            var gameState = this.CheckVertical();
            if (gameState.Winner != Shape.None)
            {
                return gameState;
            }

            gameState = this.CheckHorizontal();
            if (gameState.Winner != Shape.None)
            {
                return gameState;
            }

            gameState = this.CheckDiagonal1();
            if (gameState.Winner != Shape.None)
            {
                return gameState;
            }

            gameState = this.CheckDiagonal2();
            if (gameState.Winner == Shape.None && this.CheckAllFieldsFilled())
            {
                gameState.Draw = true;
            }

            return gameState;
        }

        private GameState CheckVertical()
        {
            var gameState = new GameState();
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
                        gameState.WinningStartField = new Point(i, 0);
                        gameState.WinningEndField = new Point(i, j);
                        gameState.Winner = currentShape;
                        return gameState;
                    }
                }
            }

            return gameState;
        }

        private GameState CheckHorizontal()
        {
            var gameState = new GameState();
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
                        gameState.WinningStartField = new Point(0, i);
                        gameState.WinningEndField = new Point(j, i);
                        gameState.Winner = currentShape;
                        return gameState;
                    }
                }
            }

            return gameState;
        }

        private GameState CheckDiagonal1()
        {
            var gameState = new GameState();
            var currentShape = this.shapes[0, 0];
            for (int i = 1; i < DrawingService.NumberOfFields; i++)
            {
                if (currentShape == Shape.None || currentShape != this.shapes[i, i])
                {
                    break;
                }

                if (i == DrawingService.NumberOfFields - 1)
                {
                    gameState.WinningStartField = new Point(0, 0);
                    gameState.WinningEndField = new Point(i, i);
                    gameState.Winner = currentShape;
                    return gameState;
                }
            }

            return gameState;
        }

        private GameState CheckDiagonal2()
        {
            var gameState = new GameState();
            var currentShape = this.shapes[0, DrawingService.NumberOfFields - 1];
            for (int i = 1; i < DrawingService.NumberOfFields; i++)
            {
                if (currentShape == Shape.None || currentShape != this.shapes[i, DrawingService.NumberOfFields - 1 - i])
                {
                    break;
                }

                if (i == DrawingService.NumberOfFields - 1)
                {
                    gameState.WinningStartField = new Point(0, DrawingService.NumberOfFields - 1);
                    gameState.WinningEndField = new Point(i, DrawingService.NumberOfFields - 1 - i);
                    gameState.Winner = currentShape;
                    return gameState;
                }
            }

            return gameState;
        }

        private bool CheckAllFieldsFilled()
        {
            for (int i = 0; i < DrawingService.NumberOfFields; i++)
            {
                for (int j = 0; j < DrawingService.NumberOfFields; j++)
                {
                    if (this.shapes[i, j] == Shape.None)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
