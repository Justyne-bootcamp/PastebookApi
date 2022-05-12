using Pastebook.Data.Models;
using Pastebook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pastebook.Web.Services
{
    public interface ILikeService
    {
        public Task<Like> Insert(Like like);
        public Task<Like> Delete(Guid likeId);
        public IEnumerable<Like> GetLikesByPostId(Guid postId);
        public Like IsPostLiked(Guid userAccountId, Guid postId);
    }
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;

        public Task<Like> Insert(Like like)
        {
            return _likeRepository.Insert(like);
        }
        public Task<Like> Delete(Guid likeId)
        {
            return _likeRepository.Delete(likeId);
        }

        public IEnumerable<Like> GetLikesByPostId(Guid postId)
        {
            return _likeRepository.GetLikesByPostId(postId);
        }

        public Like IsPostLiked(Guid userAccountId, Guid postId)
        {
            return _likeRepository.IsPostLiked(userAccountId, postId);
        }
    }
}
