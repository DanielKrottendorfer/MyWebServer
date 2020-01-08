
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
        public static void foo(Socket s,IPluginManager pm)
        {
            NetworkStream stream = new NetworkStream(s);
            IRequest r = new Request(stream);
            foreach(IPlugin p in pm.Plugins)
            {
                if(p.CanHandle(r)>0.0f)
                {
                    IResponse rs = p.Handle(r);
                    rs.Send(stream);
                    break;
                }
            }

            s.Close();
        }

        public static void tempDatabase()
        {
            DbData temper = new DbData();
            temper.genTemData();
        }

        static void Main(string[] args)
        {
            IPluginManager pm = new PluginManager();
            LoadAll(pm);
            TcpListener listener = new TcpListener(IPAddress.Any, 8081);
            DbData tester = new DbData();
            tester.tempDataInsert();
            Thread dba = new Thread(() => tempDatabase());
            dba.Start();
            listener.Start();
            while (true)
            {
                Socket s = listener.AcceptSocket();
                Thread t = new Thread(() => foo(s,pm));
                t.Start();
            }
        }

        public static void LoadAll(IPluginManager pm)
        {
            String[] files = Directory.GetFiles("./Plugins/", "*.dll");
            foreach (var s in files)
            {
                pm.Add(s.Split('/').Last());
            }
        }
    }
}