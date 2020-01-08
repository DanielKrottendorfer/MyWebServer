
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
        /// <summary>
        /// Reads data from ClientSocket finds out if there is a plugins loaded that can handle the request and ahndles it.
        /// </summary>
        /// <param name="ClientSocket"></param>
        /// <param name="PluginManager"></param>
        /// <returns>A score between 0 and 1</returns>
        public static void HandleRequest(Socket ClientSocket,IPluginManager PluginManager)
        {
            NetworkStream stream = new NetworkStream(ClientSocket);
            IRequest r = new Request(stream);
            foreach(IPlugin p in PluginManager.Plugins)
            {
                if(p.CanHandle(r)>0.0f)
                {
                    IResponse rs = p.Handle(r);
                    rs.Send(stream);
                    break;
                }
            }

            ClientSocket.Close();
        }

        /// <summary>
        /// Generates temperature data.
        /// </summary>
        public static void GenerateTempDatabase()
        {
            DbData temper = new DbData();
            temper.genTemData();
        }

        static void Main(string[] args)
        {
            IPluginManager pm = new PluginManager();
            LoadAllPlugins(pm);
            TcpListener listener = new TcpListener(IPAddress.Any, 8081);
            DbData tester = new DbData();
            tester.tempDataInsert();
            Thread dba = new Thread(() => GenerateTempDatabase());
            dba.Start();
            listener.Start();
            while (true)
            {
                Socket s = listener.AcceptSocket();
                Thread t = new Thread(() => HandleRequest(s,pm));
                t.Start();
            }
        }

        /// <summary>
        /// Loads all plugins in the ./build/Plugins directory.
        /// </summary>
        /// <param name="PluginManager"></param>
        public static void LoadAllPlugins(IPluginManager PluginManager)
        {
            String[] files = Directory.GetFiles("./Plugins/", "*.dll");
            foreach (var s in files)
            {
                PluginManager.Add(s.Split('/').Last());
            }
        }
    }
}