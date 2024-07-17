using WebApplicationDapperTest.Dtos.Stock;
using WebApplicationDapperTest.Models;

namespace WebApplicationDapperTest.Contracts
{
    public interface IStockRepository
    {
        public Task<IEnumerable<StockDto>> GetStocks();
        public Task<StockDto?> GetStocksById(int Id);
        public Task<IEnumerable<StockDto>> GetStockByString(string input);
        public Task<StockDto?> CreateStock(Stock stock);
        public Task<StockDto?> DeleteStock(int id);
        public Task<StockDto?> UpdateStock(int id, UpdateStockRequestDto updateStockDto);
    }
}
