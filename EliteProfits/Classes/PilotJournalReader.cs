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
        
        private string _filePath = string.Empty;

        public FileInfo GetMostRecentFile()
        {
            return (new DirectoryInfo(_filePath)).GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
        }

        public DateTime GetLastFileWriteTimeUtc()
        {
            return GetMostRecentFile().LastWriteTimeUtc;
        }

        public List<Dictionary<string, string>> MostRecentJournalInfo(Func<Dictionary<string, object>, bool> conditions)
        {
            var lines = new List<Dictionary<string, string>>();
            var file = GetMostRecentFile();

            using (var fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = JsonConvert.DeserializeObject<Dictionary<string, object>>(reader.ReadLine());

                        if (conditions == null || conditions(line))
                        {
                            lines.Add(line.ToDictionary(s => s.Key, y => y.Value.ToString()));
                        }
                    }
                }
            }

            return lines;
        }
    }
}
