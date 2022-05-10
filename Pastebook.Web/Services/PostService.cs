using Pastebook.Data.Models;
using Pastebook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pastebook.Web.Services
{
    public interface IPostService
    {
        public Task<IEnumerable<Post>> FindAll();
        public Task<Post> FindById(Guid id);
        public Task<Post> Insert(Post _post);
        public Task<Post> Update(Post _post);
        public Task<Post> Delete(Guid id);
    }
    public class PostService : IPostService
    {
        private IPostRepository _postRepository;
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<Post>> FindAll()
        {
            return await _postRepository.FindAll();
        }

        public async Task<Post> FindById(Guid id)
        {
            return await _postRepository.FindByPrimaryKey(id);
        }

        public async Task<Post> Insert(Post _post)
        {
            return await _postRepository.Insert(_post);
        }

        public async Task<Post> Update(Post _post)
        {
            return await _postRepository.Update(_post);
        }

        public Task<Post> Delete(Guid id)
        {
            return _postRepository.Delete(id);
        }
    }
}
