using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Enums;

namespace TicTacToe.Model
{
    public class GameState
    {
        public Shape Winner { get; set; }

        public Point WinningStartField { get; set; }

        public Point WinningEndField { get; set; }
    }
}
