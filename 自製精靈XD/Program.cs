using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 自製精靈XD
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                Application.Run(new Form1());
            }
            else
            {
                Application.Run(new Form1(args));
            }
        }
    }
}
