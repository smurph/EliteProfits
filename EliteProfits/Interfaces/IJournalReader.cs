namespace EliteProfits
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IJournalReader
    {
        FileInfo GetMostRecentFile();

        DateTime GetLastFileWriteTimeUtc();

        List<Dictionary<string, string>> MostRecentJournalInfo();
    }
}