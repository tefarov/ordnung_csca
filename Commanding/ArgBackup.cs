using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using APP.General;

namespace APP.Commanding
{
    /// <summary>
    /// This argument will set the mode to Backup
    /// </summary>
    public class ArgBackup: IArgument
    {
        Processor PRC;
        string FLD;
        byte TYP = BYT_TYPBACK;

        // this will hold the files, specified in backup.cfg
        // will be filled by configurator
        HashSet<string> SFIL;

        public ArgBackup(Processor processor)
        {
            PRC = processor;
            FLD = ArgBackup.PathFolderDefault;
            
            var cfg = new configurator(ArgBackup.PathConfig, this); cfg.Configure();
        }

        public bool TryAppend(string arg)
        {
            if (arg == "all") {
                // this adds to the paths the files, specified in the backup.cfg
                if (SFIL != null) {
                    foreach (var fil in SFIL)
                        PRC.Add(fil);

                    return true;
                }
            }
            else if (arg == "show") {
                TYP = BYT_TYPSHOW;
                PRC.Add(FLD);
                return true;
            }

            return false;
        }
        public void Apply()
        {
            if (TYP == BYT_TYPBACK) {
                if (!Directory.Exists(FLD)) Directory.CreateDirectory(FLD);
                PRC.Command = new CmdBackup() { Folder = FLD };
            }
            else if (TYP == BYT_TYPSHOW) {
                PRC.Command = new CmdLaunch();
            }
        }

        static ArgBackup()
        {
            ArgBackup.PathFolderDefault = Path.Combine(Program.Location, "backup");
            ArgBackup.PathConfig = Path.Combine(Program.Location, "Config", "backup.cfg");
        }

        public static string PathConfig { get; protected set; }
        public static string PathFolderDefault { get; protected set; }

        /// <summary>
        /// It will configure the command, in accordance with a cfg-file
        /// </summary>
        class configurator : APP.General.Configurating.AConfigurator_CFG
        {
            ArgBackup ARG;

            public configurator(string filepath, ArgBackup target) : base(filepath) { ARG = target.NotNull(); }
            protected override void setParameter(string key, string value)
            {
                if (key == "directory") {
                    if (!string.IsNullOrWhiteSpace(value))
                        ARG.FLD = value;
                }
                else if (key.StartsWith("file.") || key.StartsWith("folder.")) {
                    if (!string.IsNullOrWhiteSpace(value))
                        this.file_append(value);
                }
            }
            void file_append(string value) {
                if (ARG.SFIL == null) ARG.SFIL = new HashSet<string>();
                if (!ARG.SFIL.Contains(value))
                    ARG.SFIL.Add(value);
            }
        }
        public const byte
            BYT_TYPBACK = 0x01      // this defines the command behaviour : this will backup files and dirs
            , BYT_TYPSHOW = 0x02    // this will show the output folder
            ;
    }
}
