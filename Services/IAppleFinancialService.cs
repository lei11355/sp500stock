using System.Threading.Tasks;
using SP500StocksApp.Models;

namespace SP500StocksApp.Services
{
    public interface IAppleFinancialService
    {
        Task<AppleFinancialInfo> GetAppleFinancialInfoAsync(string symbol);
    }
}