using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using APP.General;

namespace APP.Commanding
{
    class ArgConfig : IArgument
    {
        Processor PRC;

        public ArgConfig(Processor processor)
        {
            PRC = processor;
        }

        public bool TryAppend(string arg)
        {
            if (arg == "backup") {
                PRC.Add(ArgBackup.PathConfig);
                return true;
            }
            else if (arg == "all") {
                PRC.Add(ArgBackup.PathConfig);
                return true;
            }

            return false;
        }
        public void Apply()
        {
            PRC.Command = new CmdLaunch() { IsExclusive = false, Mode = CmdLaunch.BYT_MODNOTP };
        }
    }
}
