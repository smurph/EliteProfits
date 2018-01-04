namespace EliteProfits
{
    using System;
    using System.Collections.Generic;

    public class CachedJournalInfoReader
    {
        private IJournalReader _reader;

        private List<Dictionary<string, string>> _lastFileInfo = null;
        private DateTime _lastFileInfoTimeUtc = DateTime.MinValue;

        public CachedJournalInfoReader(IJournalReader reader)
        {
            _reader = reader;
        }

        public List<Dictionary<string, string>> GetJournalInfo()
        {
            return GetJournalInfo(null);
        }

        public List<Dictionary<string, string>> GetJournalInfo(Func<Dictionary<string,object>, bool> conditions)
        {
            if (_lastFileInfo != null && _lastFileInfoTimeUtc == _reader.GetLastFileWriteTimeUtc())
            {
                return _lastFileInfo;
            }

            _lastFileInfo = _reader.MostRecentJournalInfo(conditions);
            _lastFileInfoTimeUtc = _reader.GetLastFileWriteTimeUtc();
            return _lastFileInfo;
        }
    }
}
