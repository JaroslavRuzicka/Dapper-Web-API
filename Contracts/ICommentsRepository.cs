using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationDapperTest.Dtos;
using WebApplicationDapperTest.Dtos.Comment;
using WebApplicationDapperTest.Models;
using WebApplicationDapperTest.Repository;

namespace WebApplicationDapperTest.Contracts
{
    public interface ICommentsRepository
    {
        public Task<List<CommentDto>?> GetAllComments();
        public Task<CommentDto?> GetCommentById(int id);
        public Task<CommentDto?> CreateComment(int stockId, CreateCommentDto commentDto);
    }
}