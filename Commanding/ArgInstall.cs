using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.IO;

using vb = Microsoft.VisualBasic.Interaction;

namespace APP.Commanding
{
    [ComImport]
    [Guid("00021401-0000-0000-C000-000000000046")]
    internal class ShellLink
    {
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    internal interface IShellLink
    {
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
        void GetIDList(out IntPtr ppidl);
        void SetIDList(IntPtr pidl);
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        void GetHotkey(out short pwHotkey);
        void SetHotkey(short wHotkey);
        void GetShowCmd(out int piShowCmd);
        void SetShowCmd(int iShowCmd);
        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
        void Resolve(IntPtr hwnd, int fFlags);
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    class ArgInstall : IArgument
    {
        const string
            STR_FLDAPPL = "Порядок"
            , STR_LNKBKUP = "Создать резервную копию"
            , STR_LNKBKSH = "Открыть резервную папку"
            , STR_LNKUINS = "Отменить установку"
            , STR_LNKCFGA = "Настройки"
            ;

        public const byte
            BYT_MODINSL = 0x01
            , BYT_MODUINS = 0x02
            ;

        public byte Mode = BYT_MODUINS;

        public bool TryAppend(string arg) { return false; }
        public void Apply()
        {
            string fld, app = System.Reflection.Assembly.GetExecutingAssembly().Location;

            fld = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            fld = Path.Combine(fld, STR_FLDAPPL);

            if (this.Mode == BYT_MODUINS) {
                if (Directory.Exists(fld)) Directory.Delete(fld, true);

                vb.MsgBox("Готово", Microsoft.VisualBasic.MsgBoxStyle.Information);
            }
            if (this.Mode == BYT_MODINSL) {
                if (Directory.Exists(fld)) Directory.Delete(fld, true);
                Directory.CreateDirectory(fld);

                this.make_shortcut(Path.Combine(fld, STR_LNKBKUP), app, "/backup all", app, "Создаёт резервную копию всех файлов, указанных в настройках");
                this.make_shortcut(Path.Combine(fld, STR_LNKBKSH), app, "/backup show", app, "Открыть папку, в которую происходит резервное копирование");
                this.make_shortcut(Path.Combine(fld, STR_LNKCFGA), app, "/config all", app, "Открывает файлы с настройками");
                this.make_shortcut(Path.Combine(fld, STR_LNKUINS), app, "/uninstall", null, "Удаляет менюшки из ОС");

                vb.MsgBox("Готово", Microsoft.VisualBasic.MsgBoxStyle.Information);
            }
        }

        private void make_shortcut(string path, string cmd, string args, string icon, string description = null)
        {
            IShellLink link = (IShellLink)new ShellLink();

            // setup shortcut information
            link.SetPath(cmd);
            if (!string.IsNullOrEmpty(description)) link.SetDescription(description);
            if (!string.IsNullOrWhiteSpace(args)) link.SetArguments(args);
            if (!string.IsNullOrWhiteSpace(icon)) link.SetIconLocation(icon, 0);
            
            // save it
            IPersistFile file = (IPersistFile)link;
            file.Save(path + ".lnk", false);
        }
    }
}
