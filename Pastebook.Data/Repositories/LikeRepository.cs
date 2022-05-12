using Pastebook.Data.Data;
using Pastebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Repositories
{
    public interface ILikeRepository : IBaseRepository<Like>
    {
        public IEnumerable<Like> GetAllLikesByPostId(Guid postId);
        public bool IsPostLiked(Guid userAccountId, Guid postId);
        public Like GetLikeByPostIdAndUserId(Guid userAccountId, Guid postId);
    }
    public class LikeRepository : GenericRepository<Like>, ILikeRepository
    {
        public LikeRepository(PastebookContext context) : base(context)
        {
        }

        public IEnumerable<Like> GetAllLikesByPostId(Guid postId)
        {
            var likesByPostId = this.Context.Likes
                .Select(l => l)
                .Where(l => l.PostId == postId)
                .ToList();

            return likesByPostId;
        }

        public Like GetLikeByPostIdAndUserId(Guid userAccountId, Guid postId)
        {
            var likePost = this.Context.Likes
                .Where(l => l.UserAccountId.Equals(userAccountId) && l.PostId.Equals(postId))
                .FirstOrDefault();

            return likePost;

        }

        public bool IsPostLiked(Guid userAccountId, Guid postId)
        {
            var likedPost = this.Context.Likes
                .Select(l => l)
                .Where(p => p.PostId == postId && p.UserAccountId == userAccountId)
                .FirstOrDefault();

            if(likedPost != null)
            {
                return true;
            }
            return false;
        }
    }
}
