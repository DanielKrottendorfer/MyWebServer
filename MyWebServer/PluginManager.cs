using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using BIF.SWE1.Interfaces;
using System.IO;
using System.Reflection;

namespace MyWebServer
{
    class PluginManager : IPluginManager
    {
        public PluginManager()
        {
            Plugins = new List<IPlugin>();
        }
        public IEnumerable<IPlugin> Plugins  { get; }

        public void Add(IPlugin plugin)
        {
            Plugins.Append(plugin);
        }

        public void Add(string plugin)
        {
            //Load the DLLs from the Plugins directory
            if (Directory.Exists(plugin))
            {
                string[] files = Directory.GetFiles(plugin);
                foreach (string file in files)
                {
                    if (file.EndsWith(".dll"))
                    {
                        Assembly.LoadFile(Path.GetFullPath(file));
                    }
                }
            }

            Type interfaceType = typeof(IPlugin);
            //Fetch all types that implement the interface IPlugin and are a class
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
                .ToArray();
            foreach (Type type in types)
            {
                //Create a new instance of all found types
                Plugins.Append((IPlugin)Activator.CreateInstance(type));
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
