using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using rx = System.Text.RegularExpressions.Regex;

namespace APP.Commanding
{
    /// <summary>
    /// This command will create a new version of each of the files and directories
    /// </summary>
    public class CmdBackup : ICommand
    {
        public DateTime Date = DateTime.Now;

        public void Execute(string filepath)
        {
            string pth = null;
            string dir, fil, ext;
            byte mod = BYT_MODNONE; // 1 - file , 2 - directory

            // this will define a movement mode : file or directory
            if (File.Exists(filepath)) mod = BYT_MODFILE;
            else if (Directory.Exists(filepath)) mod = BYT_MODDIRY;

            // this splits a path into parts
            dir = Path.GetDirectoryName(filepath);
            fil = H.RexIterator.Replace(Path.GetFileNameWithoutExtension(filepath), string.Empty);
            ext = Path.GetExtension(filepath);

            // this defines if the path already contains version-data of some kind
            // and then generates a new file-name with a new version-data of the same kind
            if (H.RexDat0.IsMatch(fil)) {
                pth = H.RexDat0.Replace(fil, this.Date.ToString("yyyyMMdd"), 1);
            }
            else if (H.RexDat1.IsMatch(fil)) {
                pth = H.RexDat1.Replace(fil, this.Date.ToString("yyyyMMdd"), 1);
            }
            else if (H.RexDat2.IsMatch(fil)) {
                pth = H.RexDat2.Replace(fil, this.Date.ToString("yyyyMMdd"), 1);
            }
            // this means that a file/dir doesn't contain any version-data
            // we just append it version-data
            else {
                pth = fil + "." + this.Date.ToString("yyyyMMdd");
            }

            pth = Path.Combine(this.Folder, pth) + ext;
            
            if (mod == BYT_MODFILE) File.Copy(filepath, pth, true);
            if (mod == BYT_MODDIRY) copy_directory(filepath, pth);
        }

        void copy_directory(string file, string destination)
        {
            Directory.CreateDirectory(destination);

            string trg;
            var afil = Directory.GetFiles(file);
            for (int i = 0; i < afil.Length; i++) {
                trg = Path.GetFileName(afil[i]);
                trg = Path.Combine(destination, trg);

                File.Copy(afil[i], trg);
            }
        }

        /// <summary>
        /// Directory to copy files' backups to
        /// </summary>
        public string Folder { get; set; }

        const byte
            BYT_MODNONE = 0x00
            , BYT_MODFILE = 0x01
            , BYT_MODDIRY = 0x02
            ;
    }


}
