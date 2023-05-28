using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace SimpleHttpServer {

    public class SimpleHttpServer {
        public static string fileName;

        public static void Main() {
            SimpleHttpServer.fileName = Path.GetTempPath() + "TicTacToe.log";
            Console.WriteLine(fileName);
            try
            {
                TcpListener listener = new TcpListener(8080);
                listener.Start();
                while(true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread thread = new Thread(new HttpHandler(client).Do);
                    thread.Start();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
