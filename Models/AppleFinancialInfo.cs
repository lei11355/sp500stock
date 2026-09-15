using System;
using System.Collections.Generic;

namespace SP500StocksApp.Models
{
    public class AppleFinancialInfo
    {
        public string Symbol { get; set; } = string.Empty;
        public string LongName { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public string AnalystRating { get; set; } = string.Empty;
        
        // Price Targets
        public decimal? TargetHighPrice { get; set; }
        public decimal? TargetLowPrice { get; set; }
        public decimal? TargetMeanPrice { get; set; }
        public decimal? TargetMedianPrice { get; set; }
        
        // Earnings Estimates
        public decimal? EpsCurrentYear { get; set; }
        public decimal? EpsNextYear { get; set; }
        public decimal? EpsTrailingTwelveMonths { get; set; }
        public decimal? EpsForward { get; set; }
        public DateTime? NextEarningsDate { get; set; }
        
        // Dividend History
        public List<DividendRecord> Dividends { get; set; } = new();
        
        // Stock Split History
        public List<SplitRecord> Splits { get; set; } = new();
    }
    
    public class DividendRecord
    {
        public DateTime Date { get; set; }
        public decimal Dividend { get; set; }
    }
    
    public class SplitRecord
    {
        public DateTime Date { get; set; }
        public double BeforeSplit { get; set; }
        public double AfterSplit { get; set; }
        public string Description => $"{BeforeSplit}:{AfterSplit}";
    }
}