namespace EliteProfits
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MissionData
    {
        private IJournalReader _reader;

        public MissionData(IJournalReader reader)
        {
            _reader = reader;
        }

        public List<Dictionary<string, string>> MostRecentMissionInfo()
        {
            return _reader.MostRecentJournalInfo()
                .Where(line => 
                    line.ContainsKey("event") && line.ContainsKey("Reward") && ((string)line["event"] == "MissionCompleted" || (string)line["event"] == "MissionAccepted" || (string)line["event"] == "MissionFailed")
                ).ToList();
        }

        public long TotalMissionIncome()
        {
            if (!MostRecentMissionInfo().Any(f => f["event"] == "MissionCompleted")) return 0;

            return MostRecentMissionInfo().Where(f => f["event"] == "MissionCompleted").Sum(f => Convert.ToInt64(f["Reward"]));
        }
    }
}
