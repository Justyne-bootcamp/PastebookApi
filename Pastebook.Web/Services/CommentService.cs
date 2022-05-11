using Pastebook.Data.Models;
using Pastebook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pastebook.Web.Services
{
    public interface ICommentService
    {
        public Task<IEnumerable<Comment>> FindAll();
        public Task<Comment> FindById(Guid id);
        public Task<Comment> Insert(Comment _comment);
        public Task<Comment> Update(Comment _comment);
        public Task<Comment> Delete(Guid id);
    }
    public class CommentService: ICommentService
    {
        private ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<Comment>> FindAll()
        {
            return await _commentRepository.FindAll();
        }

        public async Task<Comment> FindById(Guid id)
        {
            return await _commentRepository.FindByPrimaryKey(id);
        }

        public async Task<Comment> Insert(Comment _comment)
        {
            return await _commentRepository.Insert(_comment);
        }

        public async Task<Comment> Update(Comment _comment)
        {
            return await _commentRepository.Update(_comment);
        }

        public async Task<Comment> Delete(Guid id)
        {
            return await _commentRepository.Delete(id);
        }
    }
}
