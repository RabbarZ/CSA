using Explorer700Library;

namespace TicTacToe
{
  internal class Program
  {
    static void Main(string[] args)
    {
      var explorer = new Explorer700();

      explorer.Joystick.JoystickChanged += (s, e) =>
      {
        Console.WriteLine("daber");
      };
    }
  }
}