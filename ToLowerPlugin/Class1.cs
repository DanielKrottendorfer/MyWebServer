using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BIF.SWE1.Interfaces;
using MyWebServer;

namespace ToLowerPlugin
{
    public class Class1 : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            if (req.Url.Segments.Contains("ToLower"))
                return 1.0f;
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            IResponse r = new Response();

            string top = File.ReadAllText(@".\res\firstToLower.html");
            string bot = File.ReadAllText(@".\res\secondToLower.html");
            string mid = "";

            if(req.ContentBytes != null)
            {
                mid = req.ContentString.ToLower().Split('=').Last();
            }

            r.StatusCode = 200;
            r.SetContent(top + mid + bot);
            return r;
        }
    }
}
