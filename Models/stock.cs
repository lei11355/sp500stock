using System;

namespace SP500StocksApp.Models
{
    public class Stock
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public long Volume { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Sector { get; set; }
    }

    public class SP500Constituent
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Sector { get; set; }
    }
}