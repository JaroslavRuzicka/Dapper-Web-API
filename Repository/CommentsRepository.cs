using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Dapper;
using WebApplicationDapperTest.Context;
using WebApplicationDapperTest.Contracts;
using WebApplicationDapperTest.Dtos.Comment;
using WebApplicationDapperTest.Mappers;
using WebApplicationDapperTest.Models;

namespace WebApplicationDapperTest.Repository
{
    public class CommentsRepository : ICommentsRepository
    {
        private DapperContext _context;

        public CommentsRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<CommentDto>?> GetAllComments()
        {
            var query = "SELECT * FROM Comments";

            using var connection = _context.CreateConnection();
            IEnumerable<Comment> stocks =  await connection.QueryAsync<Comment>(query);

            if(!stocks.Any()) return null;

            return stocks.Select(x => x.ToCommentDto()).ToList();
        }
        public async Task<CommentDto?> GetCommentById(int id)
        {
            var query = "SELECT * FROM Comments WHERE Id = @ID";

            using var connection = _context.CreateConnection();
            Models.Comment comment = await connection.QueryFirstAsync<Models.Comment>(query, new {ID = id});

            if(comment == null) return null;

            return comment.ToCommentDto();
        }

        public async Task<CommentDto?> CreateComment(int stockId, CreateCommentDto commentDto)
        {
            if(commentDto == null) return null;
            
            
            
            var comment = commentDto.ToCommentFromCommentDto();

            var query = "INSERT INTO Comments(Title, Content, StockId, CreatedOn) OUTPUT Inserted.ID VALUES (@Title, @Content, @StockId, @CreatedOn)";
            
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("Title", comment.Title, System.Data.DbType.String);
            parameters.Add("Content", comment.Content, System.Data.DbType.String);
            parameters.Add("StockId", stockId, System.Data.DbType.Int32);
            parameters.Add("CreatedOn", comment.CreatedOn, System.Data.DbType.DateTime);

            int id = await connection.QueryFirstAsync<int>(query, parameters);

            var createdComment = await GetCommentById(id);

            return createdComment;
        }


    }
}