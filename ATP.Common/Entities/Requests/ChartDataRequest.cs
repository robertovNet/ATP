using System;
using ATP.Common.Enums;

namespace ATP.Common.Entities.Requests
{
    public class ChartDataRequest : ReturnRequestBase
    {
        private const string _command = "returnChartData";
        private readonly DateTime startDate;
        private readonly DateTime endDate;
        private readonly CandlestickPeriod candlestickPeriod;

        public ChartDataRequest(string currencyPair, DateTime startDate, DateTime endDate, CandlestickPeriod period) : base(_command)
        {
            CurrencyPair = currencyPair;
            this.startDate = startDate;
            this.endDate = endDate;
            candlestickPeriod = period;
        }

        public string CurrencyPair { get; set; }

        public long Start => ((DateTimeOffset)startDate).ToUnixTimeSeconds();

        public long End => ((DateTimeOffset)endDate).ToUnixTimeSeconds();

        public int Period => (int)candlestickPeriod;
    }
}
