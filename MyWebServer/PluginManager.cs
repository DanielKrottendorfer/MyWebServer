using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebServer;

namespace BIF.SWE1.Interfaces
{
    class PluginManager : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            throw new NotImplementedException();
        }

        public IResponse Handle(IRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
