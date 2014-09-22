using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using sritext;

namespace SriShell_Guess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*if (!File.Exists("lie.ini"))
                Application.Run(new Activation());
            if (File.Exists("lie.ini"))
            {
                Activation.Verify();
                Application.Run(new MainForm());
            }*/
            Application.Run(new MainForm());
        }
    }
}