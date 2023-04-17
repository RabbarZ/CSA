namespace TicTacToe.Model
{
  public class FieldCoordinate
  {
    public FieldCoordinate(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }

    public int X { get; set; }

    public int Y { get; set; }
  }
}
