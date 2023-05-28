using System;
using System.Net.Sockets;
using System.IO;

namespace SimpleHttpServer {

    public class HttpHandler {

        private TcpClient client;

        public HttpHandler(TcpClient client) {
            this.client = client;
        }

        public void Do() {
            StreamReader sr = new StreamReader(client.GetStream());
            StreamWriter sw = new StreamWriter(client.GetStream());
            Console.WriteLine("Verbindung zu " + client.Client.RemoteEndPoint);
            // Datei lesen
            string datenFile;
            using (StreamReader file = new StreamReader(SimpleHttpServer.fileName))
            {
                datenFile = file.ReadToEnd();
            }

            // Datei im HTTP-Format senden
            string request = sr.ReadLine();
            Console.WriteLine(request);
            if(request.Contains("GET"))
            {
                sw.WriteLine("HTTP/0.9 200 OK");
                sw.WriteLine("Content-type: text/plain");
                sw.WriteLine("Content-length: {0}", datenFile.Length);
                sw.WriteLine();
                sw.WriteLine(datenFile);
                sw.Flush();
            }
            client.Close();
        }
    }
}
