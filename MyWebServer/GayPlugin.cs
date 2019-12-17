using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

using BIF.SWE1.Interfaces;
namespace MyWebServer
{

    public class UselessShitTemp
    {
        public int temperatureData = 20;
        public int negSet = 1;
        public int warri = -90000;
        public DateTime timebois = DateTime.Now;

        public void itsGay()
        {
            string connstring = "Server=127.0.0.1; Port=5432; User ID=postgres; Password=postgres;Database=postgres;";
            NpgsqlConnection db = new NpgsqlConnection(connstring);
            db.Open();
            for (int x = 0; x < 10000; x++)
            {
                NpgsqlCommand cmd = new NpgsqlCommand("insert into test(temp, date) values (@p, @q)", db);
                cmd.Parameters.AddWithValue("p", temperatureData);
                cmd.Parameters.AddWithValue("q", timebois.AddHours(warri));
                // need research cmd.Prepare();
                cmd.ExecuteNonQuery();
                warri += 9;
                temperatureData += negSet;
                if (temperatureData == 30 || temperatureData == -30)
                {
                    negSet = negSet * -1;
                }
            }
            db.Close();
        }
        public void genTemData()
        {
            UselessShitTemp temper = new UselessShitTemp();
            string connstring = "Server=127.0.0.1; Port=5432; User ID=postgres; Password=postgres;Database=postgres;";
            NpgsqlConnection db = new NpgsqlConnection(connstring);
            db.Open();
            while (true)
            {
                NpgsqlCommand cmd = new NpgsqlCommand("insert into test(temp, date) values (@p, @q)", db);
                cmd.Parameters.AddWithValue("p", temperatureData);
                DateTime todayIsInYourHand = DateTime.Now;
                cmd.Parameters.AddWithValue("q", todayIsInYourHand);
                // need research cmd.Prepare();
                cmd.ExecuteNonQuery();
                temperatureData += negSet;
                if (temperatureData == 30 || temperatureData == -30)
                {
                    negSet = negSet * -1;
                }
                Thread.Sleep(1000);
                /*temperatureData += negSet;
                NpgsqlCommand cmd = new NpgsqlCommand("Select * from test WHERE temp_id = 1", db);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                liest den ersten wert der row//Console.WriteLine(reader.GetInt64(0));
                liest den zweiten wert der row//Console.WriteLine(reader.GetInt64(1));
                liest den dritten wert der row//Console.WriteLine(reader.GetDateTime(2));
                Console.Write("\n" + temperatureData + "\n");

                if (temperatureData == 30 || temperatureData == -30)
                {
                    negSet = negSet * -1;
                }
                Thread.Sleep(500);
                */
            }
        }
    }

    class GayPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            throw new NotImplementedException();
        }

        public IResponse Handle(IRequest req)
        {
            Response r = new Response();
            r.StatusCode = 200;
            var header = "<head><link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css' integrity='sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO' crossorigin='anonymous'><script src='https://code.jquery.com/jquery-3.3.1.slim.min.js' integrity='sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo' crossorigin='anonymous'></script><script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js' integrity='sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1' crossorigin='anonymous'></script><script src='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js' integrity='sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM' crossorigin='anonymous'></script></head>";
            var body = "";
            Console.Write(req.ContentString);

            if (req.ContentString == "type=temperature")
            {
                body = header + "<form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='staticdata' class='btn btn-primary btn-lg btn-block'>Statische Dateien</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='navi' class='btn btn-primary btn-lg btn-block'>Navi</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='tolower' class='btn btn-primary btn-lg btn-block'>ToLower</button></form>";
            }
            else if (req.ContentString == "type=staticdata")
            {
                body = header + "<form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='temperature' class='btn btn-primary btn-lg btn-block'>Temperatur Messung</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='navi' class='btn btn-primary btn-lg btn-block'>Navi</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='tolower' class='btn btn-primary btn-lg btn-block'>ToLower</button></form>";
            }
            else if (req.ContentString == "type=navi")
            {
                body = header + "<form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='temperature' class='btn btn-primary btn-lg btn-block'>Temperatur Messung</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='staticdata' class='btn btn-primary btn-lg btn-block'>Statische Dateien</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='tolower' class='btn btn-primary btn-lg btn-block'>ToLower</button></form>";
            }
            else if (req.ContentString == "type=tolower")
            {
                body = header + "<form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='temperature' class='btn btn-primary btn-lg btn-block'>Temperatur Messung</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='staticdata' class='btn btn-primary btn-lg btn-block'>Statische Dateien</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='navi' class='btn btn-primary btn-lg btn-block'>Navi</button></form>";
            }
            else
            {
                body = header + "<form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='temperature' class='btn btn-primary btn-lg btn-block'>Temperatur Messung</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='staticdata' class='btn btn-primary btn-lg btn-block'>Statische Dateien</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='navi' class='btn btn-primary btn-lg btn-block'>Navi</button></form><form class='p-5' action='' method='get'><button type='submit' formmethod='post' name='type' value='tolower' class='btn btn-primary btn-lg btn-block'>ToLower</button></form>";
            }

            r.SetContent(body);

            return r;
        }
    }
}
