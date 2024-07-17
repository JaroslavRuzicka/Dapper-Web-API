using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationDapperTest.Dtos.Comment;

namespace WebApplicationDapperTest.Dtos.Stock
{
    public class UpdateStockRequestDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; } 


    }
}