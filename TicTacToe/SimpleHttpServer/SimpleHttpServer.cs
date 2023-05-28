using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace SimpleHttpServer {

    public class SimpleHttpServer {
        public static string fileName;

        public static void Main() {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            SimpleHttpServer.fileName = projectDirectory + "/daten.txt";
            Console.WriteLine("Content: {0}", projectDirectory);
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
