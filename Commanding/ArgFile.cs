using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APP.Commanding
{
    /// <summary>
    /// This argument just adds a file to the file-array
    /// </summary>
    public class ArgFile : IArgument
    {
        public bool IsContinued { get { return false; } }

        public Processor PRC;
        string PTH;

        public ArgFile(Processor processor, string filepath)
        {
            PRC = processor;
            PTH = filepath;
        }

        public bool TryAppend(string arg) { return false; }
        public void Apply()
        {
            PRC.Add(PTH);
        }
    }
}
