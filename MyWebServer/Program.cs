
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using MyWebServer;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Read();
            Listen();
        }

        public static void Read()
        {
            TcpClient s = new TcpClient();
            s.Connect("dasz.at", 80);
            NetworkStream stream = s.GetStream();
            Write(stream);
            StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                Console.WriteLine(line);
            }
        }

        public static void Write(NetworkStream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine("GET / HTTP/1.1");
            sw.WriteLine("host: dasz.at");
            sw.WriteLine("connection: close");
            sw.WriteLine();
            sw.Flush();
        }

        private static void Listen()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8081);
            listener.Start();
            while (true)
            {
                Socket s = listener.AcceptSocket();
                NetworkStream stream = new NetworkStream(s);
                StreamReader sr = new StreamReader(stream);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    Console.WriteLine(line);
                    if (string.IsNullOrEmpty(line)) break;
                }

                StreamWriter sw = new StreamWriter(stream);
                var body = "<form action='' method='get'>First Value: <input type='text' name='firstValue'><br>Second Value: <input type='text' name='secondValue'><br><button type='submit'>absenden</button></div></form>";
                sw.WriteLine("HTTP/1.1 200 OK");
                sw.WriteLine("connection: close");
                sw.WriteLine("content-type: text/html");
                sw.WriteLine("content-length: " + body.Length);
                sw.WriteLine();
                sw.Write(body);
                sw.Flush();
                Console.WriteLine("flushed");
                StreamReader sd = new StreamReader(stream);
                Console.WriteLine("waiting...");
                string test = sd.ReadLine();
                Console.WriteLine("received");
                Console.Write(test);
                s.Close();
            }
        }
    }
}