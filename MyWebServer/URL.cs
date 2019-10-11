using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer
{
    public class URL : IUrl
    {

        public URL(string path)
        {
            this.Parameter = new Dictionary<string, string>();
            if (path != null)
            {
                string[] temp = path.Split('?');

                this.RawUrl = path;
                this.Path = temp[0];
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
            }
        }

        public string RawUrl { get; }

        public string Path { get; }

        public IDictionary<string, string> Parameter { get; }

        public int ParameterCount { get { return Parameter.Count; } }

        public string[] Segments => throw new NotImplementedException();

        public string FileName => throw new NotImplementedException();

        public string Extension => throw new NotImplementedException();

        public string Fragment => throw new NotImplementedException();
    }
}
