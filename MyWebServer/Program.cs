
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
using BIF.SWE1.Interfaces;

namespace Main
{
    class Program
{
        public static void foo(Socket s)
        {
            NetworkStream stream = new NetworkStream(s);
            Request r = new Request(stream);
            GayPlugin gp = new GayPlugin();
            IResponse rs = gp.Handle(r);
            rs.Send(stream);

            s.Close();
        }


        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8081);
            listener.Start();
            while (true)
            {
                Socket s = listener.AcceptSocket();
                Thread t = new Thread(() => foo(s));
                t.Start();
            }
        }
    }
}