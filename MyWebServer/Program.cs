
using System;
using MyWebServer;

namespace Main { 
    class Program
    {
        static void Main(string[] args)
        {
            URL obj = new URL(null);


            if (obj.Parameter.ContainsKey("x"))
            {
                Console.WriteLine(obj.Parameter["x"]);
            }
            if (obj.Parameter.ContainsKey("y"))
            {
                Console.Write(obj.Parameter["y"]);
            }
            Console.Read();
        }
    }
}
