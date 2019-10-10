using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using io = System.IO;

namespace APP.General.Configurating
{
    /// <summary>
    /// This abstract configurator is a base for developing Configurators that use *.cfg textfiles to configurate objects
    /// </summary>
    public abstract class AConfigurator_CFG : IConfigurator
    {
        string PTH;

        Dictionary<string, string> DVAL = new Dictionary<string, string>();

        public AConfigurator_CFG(string filepath)
        {
            if (string.IsNullOrEmpty(filepath)) throw new ArgumentException("Filepath unspecified");
            if (!io.File.Exists(filepath)) throw new ArgumentException("Configuration file '" + filepath + "' doesn't exist");

            PTH = filepath;
        }

        public virtual void Configure()
        {
            try {
                using (var MSM = new io.MemoryStream()) {

                    using (var stm = new io.FileStream(PTH, io.FileMode.Open)) {
                        stm.CopyTo(MSM);
                    }

                    MSM.Position = 0;

                    using (var rdr = new io.StreamReader(MSM)) {
                        DVAL.Clear();
                        while (!rdr.EndOfStream)
                            this.parse(rdr.ReadLine());
                    }
                }
            }
            catch (Exception ex) { throw new io.FileLoadException("Unable to read configuration file '" + PTH + "'", PTH, ex); }

            var enm = DVAL.GetEnumerator();
            while (enm.MoveNext()) {
                var prm = enm.Current;
                this.setParameter(prm.Key, prm.Value);
            }
        }

        /// <summary>
        /// Textfile-Configurators are unable to save any changes
        /// </summary>
        public virtual void Serialize() { }

        /// <summary>
        /// This will parse a read textline, extracting it's key and value data and storing it to the dictionary if needed
        /// </summary>
        /// <param name="txt">A line to parse</param>
        private void parse(string txt)
        {
            int pos = 0;
            string prp, val = null;

            // this wipes the comments
            if (txt.Contains('#')) {
                pos = txt.IndexOf('#');
                if (pos <= 0) return;

                txt = txt.Substring(0, pos);
            }

            txt = txt.Trim();
            pos = int.MaxValue;

            if (txt.Contains('\t'))
                pos = txt.IndexOf('\t');

            if (txt.Contains(' '))
                pos = Math.Min(pos, txt.IndexOf(' '));

            if (pos < int.MaxValue) {
                prp = txt.Substring(0, pos);
                val = txt.Substring(pos + 1, txt.Length - pos - 1).Trim();
            }
            else {
                prp = txt;
            }

            prp = prp.Trim().ToLowerInvariant();
            if (!string.IsNullOrEmpty(prp)) DVAL[prp] = val;
        }

        protected abstract void setParameter(string key, string value);
    }
}
