using System;
using System.Windows.Forms;

namespace CaveStarter.net
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
		    Application.SetCompatibleTextRenderingDefault(true);
		    Application.Run(new CaveStarterForm());
        }
    }
}
