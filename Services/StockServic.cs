using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SP500StocksApp.Models;

namespace SP500StocksApp.Services
{
    public interface IStockService
    {
        Task<List<SP500Constituent>> GetSP500Constituents();
        Task<Stock> GetStockQuote(string symbol);
        Task<List<Stock>> GetAllSP500Quotes();
    }

    public class StockService : IStockService
    {
        private readonly string _jsonFilePath;
        private readonly Random _random;

        public StockService(IHostEnvironment environment)
        {
            _jsonFilePath = System.IO.Path.Combine(environment.ContentRootPath, "sp500_data.json");
            _random = new Random();
        }

        private async Task<dynamic> ReadJsonData()
        {
            var jsonContent = await System.IO.File.ReadAllTextAsync(_jsonFilePath);
            return JsonSerializer.Deserialize<dynamic>(jsonContent);
        }

        public async Task<List<SP500Constituent>> GetSP500Constituents()
        {
            var constituents = new List<SP500Constituent>();
            try
            {
                var jsonContent = await System.IO.File.ReadAllTextAsync(_jsonFilePath);
                using JsonDocument doc = JsonDocument.Parse(jsonContent);
                var root = doc.RootElement;
                var constituentsArray = root.GetProperty("constituents");
                
                foreach (var item in constituentsArray.EnumerateArray())
                {
                    constituents.Add(new SP500Constituent
                    {
                        Symbol = item.GetProperty("symbol").GetString(),
                        Name = item.GetProperty("name").GetString(),
                        Sector = item.GetProperty("sector").GetString()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON: {ex.Message}");
                // Return sample data if file not found
                constituents.Add(new SP500Constituent { Symbol = "AAPL", Name = "Apple Inc.", Sector = "Technology" });
                constituents.Add(new SP500Constituent { Symbol = "MSFT", Name = "Microsoft Corp.", Sector = "Technology" });
                constituents.Add(new SP500Constituent { Symbol = "GOOGL", Name = "Alphabet Inc.", Sector = "Communication" });
            }
            return constituents;
        }

        public async Task<Stock> GetStockQuote(string symbol)
        {
            var stock = new Stock { Symbol = symbol, LastUpdated = DateTime.Now };
            try
            {
                var data = await ReadJsonData();
                if (data?.quotes?[symbol] != null)
                {
                    var quote = data.quotes[symbol];
                    stock.Price = (decimal)quote.price;
                    stock.Change = (decimal)quote.change;
                    stock.ChangePercent = (decimal)quote.changePercent;
                    stock.Volume = (long)quote.volume;
                }
                else
                {
                    stock.Price = (decimal)_random.Next(50, 500);
                    stock.Change = (decimal)(_random.NextDouble() * 10 - 5);
                    stock.ChangePercent = (decimal)(_random.NextDouble() * 4 - 2);
                    stock.Volume = _random.Next(1000000, 100000000);
                }
            }
            catch
            {
                stock.Price = (decimal)_random.Next(50, 500);
                stock.Change = (decimal)(_random.NextDouble() * 10 - 5);
                stock.ChangePercent = (decimal)(_random.NextDouble() * 4 - 2);
                stock.Volume = _random.Next(1000000, 100000000);
            }
            return stock;
        }

        public async Task<List<Stock>> GetAllSP500Quotes()
        {
            var constituents = await GetSP500Constituents();
            var stocks = new List<Stock>();
            foreach (var constituent in constituents)
            {
                var stock = await GetStockQuote(constituent.Symbol);
                stock.Name = constituent.Name;
                stock.Sector = constituent.Sector;
                stocks.Add(stock);
            }
            return stocks;
        }
    }
}