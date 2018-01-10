namespace ProfitForms
{
    using EliteProfits;
    using System;
    using System.Configuration;
    using System.Windows.Forms;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var _reader = new PilotJournalReader(ConfigurationManager.AppSettings["EliteDangerousJournalPath"]);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new frmMain(AutofacConfig.BuildContainer()));
        }
    }
}
