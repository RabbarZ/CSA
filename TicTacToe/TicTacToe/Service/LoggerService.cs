using System.Text;

namespace TicTacToe.Service
{
    public class LoggerService
    {
        private static readonly object lockObject = new();
        private readonly string path = Path.GetTempPath();
        private readonly string filename = "TicTacToe.log";

        public string Name { get; }

        public LoggerService(string name)
        {
            this.Name = name;
        }

        public void Log(string message)
        {
            var options = new FileStreamOptions
            {
                Mode = FileMode.Append,
                Access = FileAccess.Write,
                Options = FileOptions.Asynchronous,
                Share = FileShare.Read
            };
            var fullname = Path.Combine(this.path, this.filename);
            StreamWriter? writer = null;

            lock (lockObject)
            {
                try
                {
                    if (!File.Exists(fullname))
                    {
                        var stream = File.Create(fullname);
                        writer = new StreamWriter(stream, Encoding.ASCII);
                        writer.WriteLine("// Logs from TicTacToe - Team 09");
                    }

                    writer ??= new StreamWriter(fullname, Encoding.ASCII, options);

                    writer.WriteLine($"{this.Name}: {message}: {DateTime.Now}");
                    writer.Close();
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
