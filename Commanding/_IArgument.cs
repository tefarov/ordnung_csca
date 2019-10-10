using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APP.Commanding
{
    public interface IArgument
    {
        /// <summary>
        /// This will try applying some 2nd, 3rd argument. Will return true if argument is applied
        /// </summary>
        /// <param name="arg">a command-line arguemtn to be applied to the item</param>
        bool TryAppend(string arg);

        void Apply();
    }
}
