using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebServer
{
    public class Url : IUrl
    {

        public Url(string path)
        {
            this.Parameter = new Dictionary<string, string>();
            if (path != null)
            {
                this.RawUrl = path;

                string[] temp = path.Split('#');
                if (temp.Length == 2)
                {
                    this.Fragment = temp[1];
                }

                temp = temp[0].Split('?');

                if (temp.Length > 1)
                {
                    foreach (string parameter in temp[1].Split('&'))
                    {
                        string[] t = parameter.Split('=');
                        if (t.Length > 1)
                        {
                            string varName = t[0];
                            string varValue = t[1];
                            this.Parameter.Add(varName, varValue);
                        }
                    }
                }

                this.Path = temp[0];

                temp = temp[0].Split('/');

                List<string> tempSegments = new List<string>();

                if (temp.Length > 1)
                {
                    foreach (string fragment in temp)
                    {
                        if (!string.IsNullOrEmpty(fragment))
                        {
                            tempSegments.Add(fragment);
                        }
                    }
                    this.Segments = new string[tempSegments.Count()];

                    if (tempSegments.Count() > 0)
                    {
                        int i = 0;
                        foreach (string fragment in tempSegments)
                        {
                            this.Segments[i] = fragment;
                            i++;
                        }
                    }
                }
                else
                {
                    this.Segments = new string[1];
                    this.Segments[0] = temp[0];
                }


            }
        }

        public string RawUrl { get; }

        public string Path { get; }

        public IDictionary<string, string> Parameter { get; }

        public int ParameterCount { get { return Parameter.Count; } }

        public string[] Segments { get; }

        public string FileName => throw new NotImplementedException();

        public string Extension => throw new NotImplementedException();

        public string Fragment { get; }

        override public string ToString()
        {
            string s = "";
            s = s + "RawUrl: " + RawUrl + "\n";
            s = s + "Fragment: " + Fragment + "\n";
            s = s + "Path: " + Path + "\n\n";
            s += "Parameters: \n";
            foreach (KeyValuePair<string, string> kvp in Parameter)
            {
                //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
               s += string.Format("Key = {0}, Value = {1} \n", kvp.Key, kvp.Value);
            }
            s += "\n";
            s += "Segments: \n";
            foreach ( string segmet in Segments)
            {
                s += segmet;
            }
            s += "\n";
            return s;
        }

    }
}
