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
            string damn = req.Url.RawUrl.ToString();
            damn = damn.Split('/')[1];
            if (damn == "temperature" || damn == "GetTemperature")
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }


        public IResponse Handle(IRequest req)
        {
            Response r = new Response();
            r.StatusCode = 200;
            string[] urlValue = req.Url.RawUrl.Split('/');

            if (urlValue[1] == "temperature")
            {
                DateTime fromDate, toDate;
                var headerPath = @".\res\header.html";
                var graphFirstPartPath = @".\res\graphfirsttemperature.html";
                var graphSecondPartPath = @".\res\graphsecondtemperature.html";
                var tableFirstPartPath = @".\res\tablefirsttemperature.html";
                var tableSecondPartPath = @".\res\tablesecondtemperature.html";
                var tableThirdPartPath = @".\res\tablethirdtemperature.html";
                var datepage = "<input type=\"hidden\" name=\"date\" value=\"";
                DateTime dateDatePage = DateTime.Now;
                var body = "</head>";

                string[] contentArray = req.ContentString.Split('&');

                if (contentArray[0].Split('=')[0] == "type")
                {
                    if(contentArray[0].Split('=')[1] == "back")
                    {
                        string dateTest = contentArray[1].Split('=')[1];
                        toDate = Convert.ToDateTime(dateTest);
                        fromDate = toDate.AddDays(-7);
                        datepage += fromDate.ToString();

                    }
                    else if(contentArray[0].Split('=')[1] == "forward")
                    {
                        string dateTest = contentArray[1].Split('=')[1];
                        DateTime tempDate = Convert.ToDateTime(dateTest);
                        fromDate = tempDate.AddDays(7);
                        toDate = fromDate.AddDays(7);
                        datepage += fromDate.ToString();
                    }
                    else
                    {
                        datepage += dateDatePage.ToString();
                        fromDate = dateDatePage;
                        toDate = dateDatePage.AddHours(-7);
                    }
                }
                else if (contentArray[0].Split('=')[0] == "datefrom")
                {
                    //bool caught = false;
                    //try
                    //{
                        fromDate = Convert.ToDateTime(contentArray[0].Split('=')[1]);
                        toDate = Convert.ToDateTime(contentArray[1].Split('=')[1]);
                    /*}
                    catch (FormatException e)
                    {
                        caught = true;
                        Console.WriteLine("Kein Datum gewählt");
                    }
                    if(!caught)
                    {
                        fromDate = Convert.ToDateTime(contentArray[0].Split('=')[1]);
                        toDate = Convert.ToDateTime(contentArray[1].Split('=')[1]);
                    }*/
                    datepage += fromDate.ToString();
                }
                else
                {
                    datepage += dateDatePage.ToString();
                    fromDate = dateDatePage;
                    toDate = dateDatePage.AddDays(-1);
                    Console.WriteLine("FUCK ME IN THE PUSSY BAUS");
                }

                datepage += "\" />";
                Console.WriteLine(fromDate);
                Console.WriteLine(toDate);
                Console.Write(req.ContentString);
                var table = "";
                var graph = "";
                string connstring = "Server =127.0.0.1; Port=5432; User ID=postgres; Password=postgres;Database=postgres;";
                NpgsqlConnection db = new NpgsqlConnection(connstring);
                db.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("Select * from test WHERE date::date BETWEEN @p and @q", db);
                cmd.Parameters.AddWithValue("p", fromDate);
                cmd.Parameters.AddWithValue("q", toDate);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("INSTANT DROPPING BOIS");
                    DateTime itsTime = reader.GetDateTime(2);
                    string firstPart = itsTime.Month.ToString();
                    int firstInt = Int32.Parse(firstPart);
                    firstInt--;
                    firstPart = firstInt.ToString();
                    string secondPart = itsTime.Hour.ToString();
                    int secondInt = Int32.Parse(secondPart);
                    secondInt--;
                    secondPart = secondInt.ToString();
                    table += "<tr><th scope=\"row\">" + reader.GetInt64(1) + "</th><td>" + reader.GetDateTime(2) + "</td></tr>";
                    graph += "{ x: new Date( Date.UTC (" + itsTime.Year.ToString() + ", " + firstPart + "," + itsTime.Day.ToString() + "," + secondPart + "," + itsTime.Minute.ToString() + ") ), y: " + reader.GetInt64(1) + ",}, ";
                    //graph = "{ x: shitfuckhead, y: 6,},{ x: penis, y: 2,}, { x: 12.12.2019-18:15:11, y: 5,}, { x: 40, y: 7,},{ x: 50, y: 1,},{ x: 60, y: 5,}, { x: 70, y: 5,},{ x: 80, y: 2,},{ x: 90, y: 2,}";
                }
                db.Close();
                body += File.ReadAllText(graphFirstPartPath) + graph + File.ReadAllText(graphSecondPartPath) + File.ReadAllText(tableFirstPartPath) + table + File.ReadAllText(tableSecondPartPath) + datepage + File.ReadAllText(tableThirdPartPath);

                var header = File.ReadAllText(headerPath);
                body = header + body;

                r.SetContent(body);

                return r;
            }
            else if (urlValue[1] == "GetTemperature")
            {
                Console.WriteLine(req.Method);
                DateTime fucker = Convert.ToDateTime(urlValue[4] + "/" + urlValue[3] + "/" + urlValue[2]);
                var body = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><day>";

                string connstring = "Server =127.0.0.1; Port=5432; User ID=postgres; Password=postgres;Database=postgres;";
                NpgsqlConnection db = new NpgsqlConnection(connstring);
                db.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("Select * from test WHERE date::date = @p", db);
                cmd.Parameters.AddWithValue("p", fucker);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    body += "<value><time>" + reader.GetDateTime(2) + "</time><temperature>" + reader.GetInt64(1) + "</temperature></value>";
                }
                db.Close();
                body += "</day>";
                r.SetContent(body);
                return r;
            }
            else
            {
                r.SetContent("something went horribly wrong");
                return r;
            }
        }

    }
}
