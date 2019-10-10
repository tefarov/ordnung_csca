using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using APP.Commanding;

namespace APP
{
    public class Processor
    {
        public enum TBehaviour : byte
        {
            None = 0x00
            , Move = 0x01
            , Copy = 0x02
            , Backup = 0x04
        }
        public TBehaviour Behaviour = TBehaviour.Copy;
        public ICommand Command;

        HashSet<string> SFIL = new HashSet<string>();

        StringBuilder TERR = new StringBuilder(), TSUC = new StringBuilder();

        
        public void Add(string path)
        {
            if (File.Exists(path))
                SFIL.Add(path);
            else if (Directory.Exists(path))
                SFIL.Add(path);
            else {
                TERR.Append("No file: ").AppendLine(path);
                this.IsAnyErrors = true;
            }
        }

        public void Process()
        {
            if (this.Command == null) this.Command = new CmdVersionNew() { Date = DateTime.Now };

            var enm = SFIL.GetEnumerator();
            while (enm.MoveNext()) {
                try {
                    TSUC.Append("File: ").Append(enm.Current).Append(' ');
                    this.Command.Execute(enm.Current);

                    TSUC.AppendLine("ok");
                }
                catch (Exception ex) {
                    TERR.Append("Error: ").Append(ex.GetType().Name).Append(' ').AppendLine(ex.Message);
                    this.IsAnyErrors = true;
                }
            }

            if (this.IsAnyErrors) {
                Microsoft.VisualBasic.Interaction.MsgBox(TERR.ToString(), Microsoft.VisualBasic.MsgBoxStyle.Exclamation);
            }

#if DEBUG
            Microsoft.VisualBasic.Interaction.MsgBox(TSUC.ToString(), Microsoft.VisualBasic.MsgBoxStyle.OkOnly);
#endif

        }

        public bool IsAnyErrors = false;
    }
}
