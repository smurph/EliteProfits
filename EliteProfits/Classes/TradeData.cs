using System;
using System.Collections.Generic;
using System.Linq;
namespace EliteProfits
{
    public class TradeData
    {
        private IJournalReader _reader;
        private CachedJournalInfoReader _cachedJournalInfo;

        public TradeData(IJournalReader reader)
        {
            _reader = reader;
            _cachedJournalInfo = new CachedJournalInfoReader(reader);
        }
        
        public List<Dictionary<string, string>> MostRecentTradeInfo()
        {
            return _cachedJournalInfo.GetJournalInfo(line => line.ContainsKey("event") && ((string)line["event"] == "MarketBuy" || (string)line["event"] == "MarketSell"));
        }

        public int TotalTradeSellValue()
        {
            return MostRecentTradeInfo().Where(f => f["event"] == "MarketSell").Sum(f => Convert.ToInt32(f["TotalSale"]));
        }

        public int TotalTradeBuyValue()
        {
            return MostRecentTradeInfo().Where(f => f["event"] == "MarketBuy").Sum(f => Convert.ToInt32(f["TotalCost"]));
        }

        public int TotalProfits()
        {
            return TotalTradeSellValue() - TotalTradeBuyValue();
        }

        public DateTime FirstTradeTime()
        {
            return MostRecentTradeInfo().Select(t => DateTime.Parse(t["timestamp"])).OrderBy(f => f).FirstOrDefault();
        }

        public DateTime LastTradeTime()
        {
            return MostRecentTradeInfo().Select(t => DateTime.Parse(t["timestamp"])).OrderByDescending(f => f).FirstOrDefault();
        }

        public int TradeCreditsPerHour()
        {
            if (FirstTradeTime() == default(DateTime) || LastTradeTime() == default(DateTime)) return 0;

            return Convert.ToInt32(TotalProfits() / (LastTradeTime() - FirstTradeTime()).TotalHours);
        }

    }
}
