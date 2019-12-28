using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Npgsql;
using BIF.SWE1.Interfaces;
using MyWebServer;

namespace SomePlugin
{
    class GayPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            return 1.0f;
        }


        public IResponse Handle(IRequest req)
        {
            Response r = new Response();
            r.StatusCode = 200;
            var headerPath = @".\res\header.html";
            var header = File.ReadAllText(headerPath);
            var body = "</head>";
            var pluginPath = "";
            Console.Write(req.ContentString);

            if (req.ContentString == "type=temperature")
            {
                var firstPartPath = @".\res\firsttemperature.html";
                var secondPartPath = @".\res\secondtemperature.html";
                var graph = "";
                string connstring = "Server=127.0.0.1; Port=5432; User ID=postgres; Password=postgres;Database=postgres;";
                NpgsqlConnection db = new NpgsqlConnection(connstring);
                db.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("Select * from test WHERE temp_id < 10", db);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime itsTime = reader.GetDateTime(2);
                    string firstPart = itsTime.Month.ToString();
                    int firstInt = Int32.Parse(firstPart);
                    firstInt--;
                    firstPart = firstInt.ToString();
                    string secondPart = itsTime.Hour.ToString();
                    int secondInt = Int32.Parse(secondPart);
                    secondInt--;
                    secondPart = secondInt.ToString();
                    graph += "{ x: new Date( Date.UTC (" + itsTime.Year.ToString() + ", " + firstPart + "," + itsTime.Day.ToString() + "," + secondPart + "," + itsTime.Minute.ToString() + ") ), y: " + reader.GetInt64(1) + ",}, ";
                    //graph = "{ x: shitfuckhead, y: 6,},{ x: penis, y: 2,}, { x: 12.12.2019-18:15:11, y: 5,}, { x: 40, y: 7,},{ x: 50, y: 1,},{ x: 60, y: 5,}, { x: 70, y: 5,},{ x: 80, y: 2,},{ x: 90, y: 2,}";
                }
                db.Close();
                header += File.ReadAllText(firstPartPath) + graph + File.ReadAllText(secondPartPath);
                pluginPath = @".\res\temperature.html";
            }
            else if (req.ContentString == "type=staticdata")
            {
                pluginPath = @".\res\staticdata.html";
            }
            else if (req.ContentString == "type=navi")
            {
                pluginPath = @".\res\navi.html";
            }
            else if (req.ContentString == "type=tolower")
            {
                pluginPath = @".\res\tolower.html";
            }
            else
            {
                pluginPath = @".\res\noplugin.html";
            }

            body = header + File.ReadAllText(pluginPath);

            r.SetContent(body);

            return r;
        }

    }
}
