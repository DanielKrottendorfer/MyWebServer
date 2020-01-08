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
            _plugins = new List<IPlugin>();
            _loadedPluginHash = new List<int>();
        }

        private readonly static object syncObj = new object();
        private List<IPlugin> _plugins;
        public IEnumerable<IPlugin> Plugins { get { lock (syncObj) { return _plugins; } } }

        private List<int> _loadedPluginHash;

        public void Load(String file)
        {
            if (!File.Exists(file) || !file.EndsWith(".dll", true, null))
                return;

            Assembly asm = null;

            try
            {
                asm = Assembly.LoadFile(file);
            }
            catch (Exception)
            {
                // unable to load
                return;
            }

            Type pluginInfo = null;
            try
            {
                Type[] types = asm.GetTypes();
                Assembly core = AppDomain.CurrentDomain.GetAssemblies().Single(x =>
                {
                   // Console.WriteLine(x.GetName());
                    return x.GetName().Name.Equals("MyWebServer");
                });
                Type type = core.GetType("BIF.SWE1.Interfaces.IPlugin");
                foreach (var t in types)
                    if (type.IsAssignableFrom((Type)t))
                    {
                        pluginInfo = t;
                        break;
                    }

                if (pluginInfo != null)
                {
                    Object o = Activator.CreateInstance(pluginInfo);
                    IPlugin plugin = (IPlugin)o;
                    this.Add(plugin);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void LoadAll()
        {
            String[] files = Directory.GetFiles("./Plugins/", "*.dll");
            foreach (var s in files)
            {
                string p = Path.Combine(Environment.CurrentDirectory, s);
                Console.WriteLine(p);
                Load(p);
            }
        }

        public void Clear()
        {
            _plugins.Clear();
        }

        public void PrintPlugins()
        {
            Console.WriteLine(this._plugins.Count());
            foreach(IPlugin p in this._plugins)
            {
                try
                {
                    Console.WriteLine(p.ToString());
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Add(IPlugin plugin)
        {
            int hash = plugin.GetHashCode();
            if (!_loadedPluginHash.Contains(hash))
            {
                _plugins.Add(plugin);
                _loadedPluginHash.Add(hash);
            }
        }

        public void Add(string plugin)
        {
            string p = Path.Combine(Environment.CurrentDirectory, "Plugins\\" + plugin);
            Console.WriteLine(p);
            Load(p);
        }
    }
}
