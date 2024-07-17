using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDapperTest.Contracts;
using WebApplicationDapperTest.Dtos.Comment;

namespace WebApplicationDapperTest.Controlers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentsControler : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepository;

        public CommentsControler(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments() 
        {
            var modelStocks = await _commentsRepository.GetAllComments();

            if(modelStocks == null) return NotFound();

            return Ok(modelStocks);

        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCommentById([FromRoute] int id)
        {
            var modelStock = await _commentsRepository.GetCommentById(id);

            if(modelStock == null) return NotFound();

            return Ok(modelStock);
        }

        [HttpPost]
        [Route("/comments/{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute]int stockId, [FromBody] CreateCommentDto comment)
        {
            var modelStock = await _commentsRepository.CreateComment(stockId, comment);

            if(modelStock == null) return NotFound();

            return Ok(modelStock);
        }

    }
}