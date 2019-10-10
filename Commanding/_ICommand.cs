using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APP.Commanding
{
    public interface ICommand
    {
        void Execute(string filepath);
    }
}
