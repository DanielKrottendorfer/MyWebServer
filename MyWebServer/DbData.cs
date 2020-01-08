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

    public class DbData
    {
        public int negSet = 1;
        public int warri = -90000;
        public DateTime timebois = DateTime.Now;

        public void tempDataInsert()
        {
            int temperatureData = 20;
            string connstring = "Server=127.0.0.1; Port=5432; User ID=postgres; Password=postgres;Database=postgres;";
            NpgsqlConnection db = new NpgsqlConnection(connstring);
            db.Open();
            for (int x = 0; x < 10000; x++)
            {
                NpgsqlCommand cmd = new NpgsqlCommand("insert into test(temp, date) values (@p, @q)", db);
                cmd.Parameters.AddWithValue("p", temperatureData);
                cmd.Parameters.AddWithValue("q", timebois.AddHours(warri));
                cmd.ExecuteNonQuery();
                warri += 9;
                temperatureData += negSet;
                cmd.Dispose();
                if (temperatureData == 30 || temperatureData == -30)
                {
                    negSet = negSet * -1;
                }
            }
            db.Close();
        }
        public void genTemData()
        {
            int tempOne, tempTwo;
            string connstring = "Server=127.0.0.1; Port=5432; User ID=postgres; Password=postgres;Database=postgres;";
            NpgsqlConnection db = new NpgsqlConnection(connstring);
            db.Open();
            NpgsqlCommand cmddata = new NpgsqlCommand("select temp from test ORDER by temp_id Desc Limit 2", db);
            NpgsqlDataReader readerdata = cmddata.ExecuteReader();
            readerdata.Read();
            tempOne = readerdata.GetInt32(0);
            readerdata.Read();
            tempTwo = readerdata.GetInt32(0);
            if (tempOne < tempTwo)
            {
                negSet = negSet * -1;
            }
            readerdata.Close();
            cmddata.Dispose();
            db.Close();
            while (true)
            {
                tempOne += negSet;
                if (tempOne == 30 || tempOne == -30)
                {
                    negSet = negSet * -1;
                }
                db.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("insert into test(temp, date) values (@p, @q)", db);
                cmd.Parameters.AddWithValue("p", tempOne);
                DateTime todayIsInYourHand = DateTime.Now;
                cmd.Parameters.AddWithValue("q", todayIsInYourHand);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.Close();
                Thread.Sleep(60000);
            }
        }
    }
}
