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

                if (temp.Length > 0)
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
                    this.Segments = new string[0];
                    this.Segments[0] = temp[0];
                }


            }
        }

        public string RawUrl { get; }

        public string Path { get; }

        public IDictionary<string, string> Parameter { get; }

        public int ParameterCount { get { return Parameter.Count; } }

        public string[] Segments { get; }

        public string FileName { get {
                    return this.RawUrl.Split('/').Last();
                } 
        }

        public string Extension {
            get {

                string filename = this.FileName;
                string[] temp = filename.Split('.');
                if (temp.Length > 1)
                    return "." + temp.Last();
                return "";
            }
        }

        public string Fragment { get; }


    }
}
