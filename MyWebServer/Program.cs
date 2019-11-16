
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Data.SqlClient;

using MyWebServer;

namespace Main
{
    class Program
    {
        public static class temperatureData
        {
            public static int temperature = 0;
        }

        static void Main(string[] args)
        {
            Thread t = new Thread(genTemData);
            t.Start();
            Read();
            Listen();
        }

        public static void genTemData()
        {
            //pls no touching
            int negSet = 1;
            while (true)
            {
                temperatureData.temperature += negSet;
                using (SqlConnection db = new SqlConnection(@"Data Source=127.0.0.1/postgres,5432;Network Library=DBMSSOCN;Initial Catalog=postgres;User ID=postgres;Password=postgres;"))
                {
                    db.Open();
                    SqlCommand cmd = new SqlCommand(@"Select * from test", db);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        Console.Write(rd.GetInt32(0));
                        Console.Write(rd.GetDateTime(1));
                    }
                    db.Close();
                    db.Dispose();
                }
                if (temperatureData.temperature == 30 || temperatureData.temperature == -30)
                {
                    negSet = negSet * -1;
                }
                Thread.Sleep(60000);
            }

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
                //StreamReader sr = new StreamReader(stream);
                //int contentlengthInt = -1;
                //while (!sr.EndOfStream)
                //{
                //    string line = sr.ReadLine();
                //    Console.WriteLine(line);
                //    if (line.StartsWith("Content-Length"))
                //    {
                //        string[] contentlengthTest = line.Split(' ');
                //        Console.WriteLine(contentlengthTest[1]);
                //    }
                //    if (string.IsNullOrEmpty(line)) break;
                //}

                //if(contentlengthInt > 0)
                //{
                //    byte[] bodyStream = new byte[contentlengthInt];
                //    char[] buffer = new char[contentlengthInt];
                //    sr.Read(buffer, 0, contentlengthInt);
                //    bodyStream = Encoding.UTF8.GetBytes(buffer);
                //    Console.WriteLine("Content:\n" + Encoding.UTF8.GetString(bodyStream));
                //    contentlengthInt = -1;
                //}
                Request r = new Request(stream);
                StreamWriter sw = new StreamWriter(stream);
                var header = "<head><link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css' integrity='sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO' crossorigin='anonymous'><script src='https://code.jquery.com/jquery-3.3.1.slim.min.js' integrity='sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo' crossorigin='anonymous'></script><script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js' integrity='sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1' crossorigin='anonymous'></script><script src='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js' integrity='sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM' crossorigin='anonymous'></script></head>";
                var body = "";
                Console.Write(r.ContentString);

                if(r.ContentString == "type=temperature")
                {
                    body = header + "<form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='staticdata' class='btn btn-primary btn-lg btn-block'>Statische Dateien</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='navi' class='btn btn-primary btn-lg btn-block'>Navi</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='tolower' class='btn btn-primary btn-lg btn-block'>ToLower</button></form>";
                }
                else if(r.ContentString == "type=staticdata")
                {
                    body = header + "<form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='temperature' class='btn btn-primary btn-lg btn-block'>Temperatur Messung</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='navi' class='btn btn-primary btn-lg btn-block'>Navi</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='tolower' class='btn btn-primary btn-lg btn-block'>ToLower</button></form>";
                }
                else if(r.ContentString == "type=navi")
                {
                    body = header + "<form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='temperature' class='btn btn-primary btn-lg btn-block'>Temperatur Messung</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='staticdata' class='btn btn-primary btn-lg btn-block'>Statische Dateien</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='tolower' class='btn btn-primary btn-lg btn-block'>ToLower</button></form>";
                }
                else if(r.ContentString == "type=tolower")
                {
                    body = header + "<form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='temperature' class='btn btn-primary btn-lg btn-block'>Temperatur Messung</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='staticdata' class='btn btn-primary btn-lg btn-block'>Statische Dateien</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='navi' class='btn btn-primary btn-lg btn-block'>Navi</button></form>";
                }
                else
                {
                    body = header + "<form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='temperature' class='btn btn-primary btn-lg btn-block'>Temperatur Messung</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='staticdata' class='btn btn-primary btn-lg btn-block'>Statische Dateien</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='navi' class='btn btn-primary btn-lg btn-block'>Navi</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='tolower' class='btn btn-primary btn-lg btn-block'>ToLower</button></form>";
                }

                sw.WriteLine("HTTP/1.1 200 OK");
                sw.WriteLine("connection: close");
                sw.WriteLine("content-type: text/html");
                sw.WriteLine("content-length: " + body.Length);
                sw.WriteLine();
                sw.Write(body);
                Console.Write("\n");
                Console.Write(temperatureData.temperature);
                Console.Write("\n");
                sw.Flush();
                s.Close();
            }
        }
    }
}