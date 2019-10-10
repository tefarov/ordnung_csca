using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APP.Commanding
{
    /// <summary>
    /// This argument just adds a file to the file-array
    /// </summary>
    public class ArgVersionNew : IArgument
    {
        Processor PRC;
        DateTime DAT;
        public CmdVersionNew.TBehaviour Behaviour = CmdVersionNew.TBehaviour.Copy;

        public ArgVersionNew(Processor processor)
        {
            PRC = processor;
            DAT = DateTime.Now;
        }

        public bool TryAppend(string arg)
        {
            DateTime val;
            if (DateTime.TryParse(arg.Trim(), out val)) {
                DAT = val;
                return true;
            }

            return false;
        }
        public void Apply()
        {
            PRC.Command = new CmdVersionNew() { Date = DAT, Behaviour = this.Behaviour };
        }
    }
}
