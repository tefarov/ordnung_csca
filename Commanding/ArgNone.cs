using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APP.Commanding
{
    class ArgNone : IArgument
    {
        public bool IsContinued { get { return false; } }

        public void Apply() { }
        public bool TryAppend(string arg) { return false; }
    }
}
