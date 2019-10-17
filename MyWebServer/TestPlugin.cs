using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    public class TestPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            return 0.5f;
        }

        public IResponse Handle(IRequest req)
        {
            return new Response();
        }
    }
}
