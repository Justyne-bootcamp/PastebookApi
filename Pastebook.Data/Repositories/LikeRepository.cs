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
        public IEnumerable<Like> GetLikesByPostId(Guid postId);
        public Like IsPostLiked(Guid userAccountId, Guid postId);
    }
    public class LikeRepository : GenericRepository<Like>, ILikeRepository
    {
        public LikeRepository(PastebookContext context) : base(context)
        {
        }

        public IEnumerable<Like> GetLikesByPostId(Guid postId)
        {
            var likesByPostId = this.Context.Likes
                .Where(x => x.PostId == postId)
                .ToList();

            return likesByPostId;
        }

        public Like IsPostLiked(Guid userAccountId, Guid postId)
        {
            var likedPost = this.Context.Likes
                .Where(p => p.PostId == postId && p.UserAccountId == userAccountId)
                .FirstOrDefault();

            return likedPost;
        }
    }
}
