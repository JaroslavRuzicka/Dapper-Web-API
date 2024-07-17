using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDapperTest.Contracts;
using WebApplicationDapperTest.Dtos.Comment;
using WebApplicationDapperTest.Dtos.Stock;
using WebApplicationDapperTest.Mappers;

namespace WebApplicationDapperTest.Controlers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockControler : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        public StockControler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            var modelStock = await _stockRepository.GetStocks();

            if(!modelStock.Any())
            {
                return NotFound();
            }

            return Ok(modelStock);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById([FromRoute] string id)
        {
            int ID;
            try
            {
                ID = Int32.Parse(id);
            }
            catch
            {
                return NotFound();
            }

            var modelStock = await _stockRepository.GetStocksById(ID);

            if (modelStock == null)
            {
                return NotFound();
            }

            return Ok(modelStock);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetStockByString([FromRoute] string name)
        {
            IEnumerable<StockDto> modelStock = await _stockRepository.GetStockByString(name);

            if(!modelStock.Any())
            {
                return NotFound();
            }

            return Ok(modelStock);

        }


        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
        {
            var modelStock = stockDto.ToStockFromCreateDto();

            StockDto? createdStock = await _stockRepository.CreateStock(modelStock);

            if(createdStock == null) return NotFound();

            return CreatedAtAction(nameof(GetStockById), new {id = createdStock.Id,}, createdStock);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id) 
        {
            var modelStock = await _stockRepository.DeleteStock(id);

            if(modelStock == null) return NotFound();

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockDto)
        {
            var modelStock = await _stockRepository.UpdateStock(id, updateStockDto);

            if(modelStock == null) return NotFound();

            return Ok(modelStock);
        }


    }
}
