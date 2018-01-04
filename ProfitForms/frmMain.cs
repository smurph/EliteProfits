using Autofac;
using EliteProfits;
using System;
using System.Windows.Forms;

namespace ProfitForms
{
    public partial class frmMain : Form
    {
        private IJournalReader _reader;
        private TradeData _marketData;
        private MissionData _missionData;

        public frmMain(IContainer container)
        {
            InitializeComponent();
            _marketData = container.Resolve<TradeData>();
            _missionData = container.Resolve<MissionData>();
            _reader = container.Resolve<IJournalReader>();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {            
            // Start immediately
            timer1_Tick(null, null);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblLastUpdated.Text = $"{_reader.GetLastFileWriteTimeUtc().ToLocalTime()}";

            lblProfit.Text = $"{_marketData.TotalProfits():n0} Cr";
            lblCreditsPerHour.Text = $"{_marketData.TradeCreditsPerHour():n0} Cr/hr";

            lblMissionCreditsEarned.Text = $"{_missionData.TotalMissionIncome():n0} Cr";
        }
    }
}
