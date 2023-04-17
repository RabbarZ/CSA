using System.Drawing;
using TicTacToe.Enums;

namespace TicTacToe.Model
{
    public class GameState
    {
        public Shape Winner { get; set; }

        public bool Draw { get; set; }

        public FieldCoordinate WinningStartField { get; set; }

        public FieldCoordinate WinningEndField { get; set; }
    }
}
