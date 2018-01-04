namespace EliteProfits
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class PilotJournalReader : IJournalReader
    {
        public PilotJournalReader(string filePath)
        {
            _filePath = filePath;
        }

        private List<Dictionary<string, string>> _journalInfo = null;
        private DateTime _lastJournalInfoTime = DateTime.MinValue;
        
        private string _filePath = string.Empty;

        public FileInfo GetMostRecentFile()
        {
            return (new DirectoryInfo(_filePath)).GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
        }

        public DateTime GetLastFileWriteTimeUtc()
        {
            return GetMostRecentFile().LastWriteTimeUtc;
        }

        public List<Dictionary<string, string>> MostRecentJournalInfo()
        {
            if (_journalInfo != null && GetLastFileWriteTimeUtc() <= _lastJournalInfoTime)
            {
                return _journalInfo;
            }

            var lines = new List<Dictionary<string, string>>();
            var file = GetMostRecentFile();

            using (var fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = JsonConvert.DeserializeObject<Dictionary<string, object>>(reader.ReadLine());
                        
                        lines.Add(line.ToDictionary(s => s.Key, y => y.Value.ToString()));
                    }
                }
            }

            _journalInfo = lines;
            _lastJournalInfoTime = file.LastWriteTimeUtc;
            return lines;
        }
    }
}
