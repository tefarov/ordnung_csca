using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using APP.Commanding;

namespace APP
{
    /// <summary>
    /// This configurator configurates a Processor, based on the command-line arguments
    /// </summary>
    public class Configurator
    {
        Processor PRC;
        public Configurator(Processor parent) { PRC = parent; }

        public IArgument GetArgument(string arg)
        {
            arg = arg.Trim();

            if (false) { }
            else if (arg == "/u" || arg == "/upd" || arg == "/update")
                return new ArgVersionNew(PRC) { Behaviour = CmdVersionNew.TBehaviour.Move };
            else if (arg == "/n" || arg == "/new")
                return new ArgVersionNew(PRC) { Behaviour = CmdVersionNew.TBehaviour.Copy };
            else if (arg == "/b" || arg == "/bkp" || arg == "/backup")
                return new ArgBackup(PRC);
            else if (arg == "/install")
                return new ArgInstall() { Mode = ArgInstall.BYT_MODINSL };
            else if (arg == "/uninstall")
                return new ArgInstall() { Mode = ArgInstall.BYT_MODUINS };
            else if (arg == "/config")
                return new ArgConfig(PRC);

            return new ArgFile(PRC, arg);
        }
    }
}
