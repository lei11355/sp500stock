namespace SP500StocksApp.Models
{
    public class StockPriceHistory
    {
        public string Symbol { get; set; } = string.Empty;
        public List<PricePoint> Prices { get; set; } = new();
    }

    public class PricePoint
    {
        public DateTime Date { get; set; }
        public decimal Close { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public long Volume { get; set; }
    }
}