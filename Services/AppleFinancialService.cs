using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SP500StocksApp.Models;

namespace SP500StocksApp.Services
{
    public class AppleFinancialService : IAppleFinancialService
    {
        public async Task<AppleFinancialInfo> GetAppleFinancialInfoAsync(string symbol)
        {
            // Return data for Apple (AAPL)
            var info = new AppleFinancialInfo
            {
                Symbol = symbol,
                LongName = "Apple Inc.",
                CurrentPrice = 175.50m,
                AnalystRating = "Buy",
                TargetHighPrice = 210.00m,
                TargetLowPrice = 160.00m,
                TargetMeanPrice = 185.00m,
                TargetMedianPrice = 187.50m,
                EpsCurrentYear = 6.55m,
                EpsNextYear = 7.25m,
                EpsTrailingTwelveMonths = 6.16m,
                EpsForward = 6.70m,
                NextEarningsDate = new DateTime(2024, 7, 25)
            };
            
            // Add dividend history (Apple pays quarterly)
            info.Dividends.AddRange(new List<DividendRecord>
            {
                new DividendRecord { Date = new DateTime(2024, 2, 9), Dividend = 0.24m },
                new DividendRecord { Date = new DateTime(2023, 11, 10), Dividend = 0.24m },
                new DividendRecord { Date = new DateTime(2023, 8, 11), Dividend = 0.24m },
                new DividendRecord { Date = new DateTime(2023, 5, 12), Dividend = 0.24m },
                new DividendRecord { Date = new DateTime(2023, 2, 10), Dividend = 0.23m },
                new DividendRecord { Date = new DateTime(2022, 11, 4), Dividend = 0.23m },
                new DividendRecord { Date = new DateTime(2022, 8, 5), Dividend = 0.23m },
                new DividendRecord { Date = new DateTime(2022, 5, 6), Dividend = 0.23m }
            });
            
            // Add stock split history
            info.Splits.AddRange(new List<SplitRecord>
            {
                new SplitRecord { Date = new DateTime(2020, 8, 31), BeforeSplit = 4, AfterSplit = 1 },
                new SplitRecord { Date = new DateTime(2014, 6, 9), BeforeSplit = 7, AfterSplit = 1 },
                new SplitRecord { Date = new DateTime(2005, 2, 28), BeforeSplit = 2, AfterSplit = 1 },
                new SplitRecord { Date = new DateTime(2000, 6, 21), BeforeSplit = 2, AfterSplit = 1 }
            });
            
            return info;
        }
    }
}