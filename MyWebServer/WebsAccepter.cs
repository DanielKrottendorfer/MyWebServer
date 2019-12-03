
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Data.SqlClient;
using Npgsql;

using MyWebServer;

namespace MyWebServer
{
    class WebsAccepter
    {
        static void Main1(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8081);
            listener.Start();
            Socket s = listener.AcceptSocket();
            NetworkStream stream = new NetworkStream(s);
            Request r = new Request(stream);
            Console.WriteLine(r.toString());
            Console.Read();
        }
    }
}
