using Explorer700Library;
using TicTacToe.Service;

namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var service = new TicTacToeService(new Explorer700());

            service.StartGame();

            Console.ReadKey();
        }
    }
}