using Microsoft.AspNetCore.Mvc;
using SP500StocksApp.Models;
using SP500StocksApp.Services;

namespace SP500StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IAppleFinancialService _appleFinancialService;
        private readonly IStockChartService _stockChartService;

        public HomeController(
            IStockService stockService,           // ← No extra 'S'
            IAppleFinancialService appleFinancialService,
            IStockChartService stockChartService)
        {
            _stockService = stockService;
            _appleFinancialService = appleFinancialService;
            _stockChartService = stockChartService;
        }

        public async Task<IActionResult> Index(string sector = null, string searchString = null)
        {
            var stocks = await _stockService.GetSP500Constituents();
            
            var sectors = stocks.Select(s => s.Sector).Distinct().OrderBy(s => s).ToList();
            ViewBag.Sectors = sectors ?? new List<string>();
            ViewBag.SelectedSector = sector;
            ViewBag.SearchString = searchString;
            
            if (!string.IsNullOrEmpty(sector))
                stocks = stocks.Where(s => s.Sector == sector).ToList();
            if (!string.IsNullOrEmpty(searchString))
                stocks = stocks.Where(s => 
                    s.Symbol.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            
            ViewBag.TotalCount = stocks.Count;
            return View(stocks);
        }

        [HttpGet]
        public async Task<JsonResult> GetStockChart(string symbol, int days = 30)
        {
            var history = await _stockChartService.GetStockPriceHistoryAsync(symbol, days);
            return Json(new
            {
                symbol = history.Symbol,
                dates = history.Prices.Select(p => p.Date.ToString("yyyy-MM-dd")).ToList(),
                prices = history.Prices.Select(p => p.Close).ToList()
            });
        }

        public async Task<IActionResult> Quotes(string sector = null, string searchString = null)
        {
            var stocks = await _stockService.GetAllSP500Quotes();
            var sectors = stocks.Select(s => s.Sector).Distinct().OrderBy(s => s).ToList();
            ViewBag.Sectors = sectors ?? new List<string>();
            ViewBag.SelectedSector = sector;
            ViewBag.SearchString = searchString;
            
            if (!string.IsNullOrEmpty(sector))
                stocks = stocks.Where(s => s.Sector == sector).ToList();
            if (!string.IsNullOrEmpty(searchString))
                stocks = stocks.Where(s => 
                    s.Symbol.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            
            ViewBag.TotalCount = stocks.Count;
            return View(stocks);
        }

        public async Task<IActionResult> Search(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
                return RedirectToAction("Index");

            var stock = await _stockService.GetStockQuote(symbol.ToUpper());
            var constituents = await _stockService.GetSP500Constituents();
            var constituent = constituents.Find(c => c.Symbol == symbol.ToUpper());
            
            if (constituent != null)
            {
                stock.Name = constituent.Name;
                stock.Sector = constituent.Sector;
            }

            return View(stock);
        }

        public async Task<IActionResult> AppleDetails()
        {
            var info = await _appleFinancialService.GetAppleFinancialInfoAsync("AAPL");
            return View(info);
        }
    }
}