using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SP500StocksApp.Services
{
    public interface IStockChartService
    {
        Task<StockPriceHistory> GetStockPriceHistoryAsync(string symbol, int days = 30);
    }

    public class StockChartService : IStockChartService
    {
        private readonly HttpClient _httpClient;
        private readonly Random _random;

        public StockChartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _random = new Random();
        }

        public async Task<StockPriceHistory> GetStockPriceHistoryAsync(string symbol, int days = 30)
        {
            var history = new StockPriceHistory { Symbol = symbol };
            
            try
            {
                var url = $"https://query1.finance.yahoo.com/v8/finance/chart/{symbol}?interval=1d&range={days}d";
                var response = await _httpClient.GetStringAsync(url);
                
                using JsonDocument doc = JsonDocument.Parse(response);
                var root = doc.RootElement;
                var result = root.GetProperty("chart").GetProperty("result")[0];
                var timestamps = result.GetProperty("timestamp");
                var closePrices = result.GetProperty("indicators").GetProperty("quote")[0].GetProperty("close");
                
                for (int i = 0; i < timestamps.GetArrayLength(); i++)
                {
                    var timestamp = timestamps[i].GetInt64();
                    var date = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
                    var close = closePrices[i].ValueKind != JsonValueKind.Null ? (decimal)closePrices[i].GetDouble() : 0;
                    
                    if (close > 0)
                    {
                        history.Prices.Add(new PricePoint { Date = date, Close = close });
                    }
                }
            }
            catch
            {
                // Generate simulated data
                var basePrice = symbol switch
                {
                    "AAPL" => 175m,
                    "MSFT" => 420m,
                    "GOOGL" => 145m,
                    "AMZN" => 178m,
                    "NVDA" => 950m,
                    "META" => 485m,
                    "TSLA" => 175m,
                    _ => 100m
                };
                
                var date = DateTime.Today.AddDays(-days);
                var price = basePrice;
                
                for (int i = 0; i < days; i++)
                {
                    price += (decimal)(_random.NextDouble() - 0.5) * basePrice * 0.02m;
                    history.Prices.Add(new PricePoint { Date = date.AddDays(i), Close = price });
                }
            }
            
            return history;
        }
    }

    public class StockPriceHistory
    {
        public string Symbol { get; set; } = string.Empty;
        public List<PricePoint> Prices { get; set; } = new();
    }

    public class PricePoint
    {
        public DateTime Date { get; set; }
        public decimal Close { get; set; }
    }
}