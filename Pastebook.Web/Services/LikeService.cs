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
        public Like GetLikeByPostIdAndUserId(Guid userAccountId, Guid postId);
        public IEnumerable<Like> GetAllLikesByPostId(Guid postId);
        public bool IsPostLiked(Guid userAccountId, Guid postId);
    }
    public class LikeService : ILikeService
    {
        private ILikeRepository _likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public Task<Like> Insert(Like like)
        {
            return _likeRepository.Insert(like);
        }
        public Task<Like> Delete(Guid likeId)
        {
            return _likeRepository.Delete(likeId);
        }

        public Like GetLikeByPostIdAndUserId(Guid userAccountId, Guid postId)
        {
            return _likeRepository.GetLikeByPostIdAndUserId(userAccountId, postId);
        }

        public IEnumerable<Like> GetAllLikesByPostId(Guid postId)
        {
            return _likeRepository.GetAllLikesByPostId(postId);
        }

        public bool IsPostLiked(Guid userAccountId, Guid postId)
        {
            return _likeRepository.IsPostLiked(userAccountId, postId);
        }
    }
}
