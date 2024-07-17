using WebApplicationDapperTest.Models;
using WebApplicationDapperTest.Mappers;
using WebApplicationDapperTest.Dtos;
using Dapper;
using WebApplicationDapperTest.Context;
using WebApplicationDapperTest.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDapperTest.Dtos.Stock;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Quic;
using System.Collections.Immutable;

namespace WebApplicationDapperTest.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly DapperContext _context;

        public StockRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockDto>> GetStocks()
        {
            var queryStocks = "SELECT * FROM Stocks";
            var queryComments = "SELECT * FROM Comments";

            using var connection = _context.CreateConnection();


            var dictionary = new Dictionary<int, Stock>();

            var stocks = await connection.QueryAsync<Stock>(queryStocks);
            var comments = await connection.QueryAsync<Comment>(queryComments);

            foreach(var stock in stocks)
            {
                stock.Comment = comments.Where(x => x.StockId == stock.Id).ToList();
            }

            return stocks.ToList().Select(s => s.ToStockDto()); 
        }

        public async Task<StockDto?> GetStocksById(int Id)
        {
            var queryStocks = "SELECT * FROM Stocks WHERE Id = @ID";
            var queryComments = "SELECT * FROM Comments WHERE StockId = @ID";
            using var connection = _context.CreateConnection();

            var stock = await connection.QueryFirstOrDefaultAsync<Stock>(queryStocks, new { ID = Id });
            var comments = await connection.QueryAsync<Comment>(queryComments, new { ID = Id });

            if(stock == null) return null;

            stock.Comment = comments.Where(x => x.StockId == stock.Id).ToList();

            return stock.ToStockDto();
        }

        public async Task<IEnumerable<StockDto>> GetStockByString(string input)
        {
            var queryStocks = "SELECT * FROM Stocks WHERE Symbol LIKE @INPUT";
            var queryComments = "SELECT * FROM Comments WHERE StockId IN @idList";

            using var connection = _context.CreateConnection();
            var stocks = await connection.QueryAsync<Stock>(queryStocks, new { INPUT = input + "%" });
            
            List<int> selectedStockIds = [];
            
            foreach(var stock in stocks)
            {
                int id = stock.Id;
                selectedStockIds.Add(id);
            }

            var comments = await connection.QueryAsync<Comment>(queryComments, new { idList = selectedStockIds});

            foreach(var stock in stocks)
            {
                stock.Comment = comments.Where(x => x.StockId == stock.Id).ToList();
            }

            var result = stocks.Select(s => s.ToStockDto());

            return result;
        }

        public async Task<StockDto?> CreateStock(Stock stock)
        {
            
            var query = @"INSERT INTO Stocks (Symbol, Company, Purchase, LastDiv, Industry, MarketCap) 
            OUTPUT Inserted.ID
            VALUES (@Symbol, @Company, @Purchase, @LastDiv, @Industry, @MarketCap)";

            using var connection = _context.CreateConnection();
            var dataObject = await connection.QueryFirstAsync(query, 
                new{Symbol = stock.Symbol, Company = stock.Company, Purchase = stock.Purchase, LastDiv = stock.LastDiv, 
                Industry = stock.Industry, MarketCap = stock.MarketCap});
            
            int id = dataObject.ID;

            var createdStock = await GetStocksById(id);

            if(createdStock == null) return null;

            return createdStock;
           
        }

        public async Task<StockDto?> DeleteStock(int id)
        {
            
            var selectedStock = await GetStocksById(id);

            if(selectedStock == null) return null;
            
            var query = "DELETE FROM Stocks WHERE ID = @ID";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new {ID = id});
            
            return selectedStock;
        }

        public async Task<StockDto?> UpdateStock(int id, UpdateStockRequestDto updateStockDto)
        {
            
            var selectedStock = await GetStocksById(id);
            
            if(selectedStock == null) return null;

            var query = @"UPDATE Stocks SET 
            Symbol = @Symbol, Company = @Company, Purchase = @Purchase, LastDiv = @LastDiv, Industry = @Industry, MarketCap = @MarketCap
            WHERE Id = @ID";

            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("Symbol", updateStockDto.Symbol, System.Data.DbType.String);
            parameters.Add("Company", updateStockDto.Company, System.Data.DbType.String);
            parameters.Add("Purchase", updateStockDto.Purchase, System.Data.DbType.Decimal);
            parameters.Add("LastDiv", updateStockDto.LastDiv, System.Data.DbType.Decimal);
            parameters.Add("Industry", updateStockDto.Industry, System.Data.DbType.String);
            parameters.Add("MarketCap", updateStockDto.MarketCap, System.Data.DbType.Int64);
            parameters.Add("ID", id, System.Data.DbType.Int32);
            await connection.ExecuteAsync(query, parameters);

            var selectedStock2 = await GetStocksById(id);
            
            if(selectedStock2 == null) return null;

            return selectedStock2;

        }

    }
}
