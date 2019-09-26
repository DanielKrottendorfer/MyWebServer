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
        public string RawUrl => throw new NotImplementedException();

        public string Path => throw new NotImplementedException();

        public IDictionary<string, string> Parameter => throw new NotImplementedException();

        public int ParameterCount => throw new NotImplementedException();

        public string[] Segments => throw new NotImplementedException();

        public string FileName => throw new NotImplementedException();

        public string Extension => throw new NotImplementedException();

        public string Fragment => throw new NotImplementedException();
    }
}
