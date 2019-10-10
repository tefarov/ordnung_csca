using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using rx = System.Text.RegularExpressions.Regex;
using vb = Microsoft.VisualBasic.Interaction;

namespace APP.Commanding
{
    /// <summary>
    /// This command will create a new version of each of the files and directories
    /// </summary>
    public class CmdLaunch : ICommand
    {
        public byte Mode = BYT_MODCCMDL;

        /// <summary>
        /// This tells that the command should be ran only once
        /// </summary>
        public bool IsExclusive = true;
        /// <summary>
        /// Count times launched
        /// </summary>
        int CNT = 0;

        public void Execute(string filepath)
        {
            string cmd, args = null;

            if (this.IsExclusive && ++CNT > 1) return;

            // this will define a movement mode : file or directory
            if (File.Exists(filepath)) { cmd = filepath; }
            else if (Directory.Exists(filepath)) { cmd = filepath; }
            else {
                throw new FileNotFoundException("Путь не найден: " + filepath);
            }

            if (this.Mode == BYT_MODNOTP) {
                args = cmd;
                cmd = "notepad";
            }

            System.Diagnostics.Process.Start(cmd, args);
        }

        public const byte
            BYT_MODCCMDL = 0x01 // command-line execution
            , BYT_MODNOTP = 0x02 // notepad
            ;
    }
}
