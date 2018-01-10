namespace ProfitForms
{
    using Autofac;
    using EliteProfits;
    using System;
    using System.IO;
    using System.Windows.Forms;

    public partial class frmMain : Form
    {
        private PilotJournalReader _reader;
        private TradeData _marketData;
        private MissionData _missionData;

        public frmMain(IContainer container)
        {
            InitializeComponent();
            _marketData = container.Resolve<TradeData>();
            _missionData = container.Resolve<MissionData>();
            _reader = container.Resolve<PilotJournalReader>();

            SetupFileSystemWatch();

            UpdateLabels(null, null);
        }

        private void SetupFileSystemWatch()
        {
            var watcher = new FileSystemWatcher()
            {
                Path = _reader.FilePath,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "Journal*.log",
                EnableRaisingEvents = true
            };

            watcher.Changed += new FileSystemEventHandler(UpdateLabels);
        }

        private delegate void SetTextDelegate(Label label, string text);

        private void SetLabelText_ThreadSafe(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                var d = new SetTextDelegate(SetLabelText_ThreadSafe);
                Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Text = text;
            }
        }

        private void UpdateLabels(object sender, EventArgs e)
        {
            SetLabelText_ThreadSafe(lblLastUpdated, _reader.GetLastFileWriteTimeUtc().ToLocalTime().ToString());

            SetLabelText_ThreadSafe(lblProfit, $"{_marketData.TotalProfits():n0} Cr");

            SetLabelText_ThreadSafe(lblCreditsPerHour, $"{_marketData.TradeCreditsPerHour():n0} Cr/hr");

            SetLabelText_ThreadSafe(lblMissionCreditsEarned, $"{_missionData.TotalMissionIncome():n0} Cr");
        }
    }

}
