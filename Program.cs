using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using APP.Commanding;

namespace APP
{
    class Program
    {
        static void Main(string[] args)
        {

            Processor prc = new Processor();
            Configurator cfg = new Configurator(prc);

            string arg;
            IArgument itm = new ArgNone();
            for (int i = 0; i < args.Length; i++) {
                arg = args[i];
#if DEBUG
                Console.WriteLine(arg);
#endif
                if (itm.TryAppend(arg)) continue;

                itm.Apply();
                itm = cfg.GetArgument(arg);
            }
            itm.Apply();

            Console.WriteLine();
            prc.Process();

#if DEBUG
            Console.ReadLine();
#else
            if (prc.IsAnyErrors)
                Console.ReadLine();
#endif
        }

        /// <summary>
        /// Folder-path of the application 
        /// </summary>
        public static string Location { get { return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); } }
    }
}
