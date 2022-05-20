using Microsoft.EntityFrameworkCore;
using Pastebook.Data.Data;
using Pastebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastebook.Data.Models.DataTransferObjects;

namespace Pastebook.Data.Repositories
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        public IEnumerable<PostDTO> GetAllNewsfeedPost(Guid userAccountId, List<Guid> friendsList);
        public Guid GetUserAccountIdByPost(Guid postId);
        public IEnumerable<PostDTO> GetTimelinePosts(Guid userAccountId, string sessionId, string username);

    }
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(PastebookContext context) : base(context)
        {
            this.Context = context;
        }

        public IEnumerable<PostDTO> GetAllNewsfeedPost(Guid userAccountId, List<Guid> friendsList)
        {
            var allPosts = this.Context.Posts.Join(
                this.Context.UserAccounts,
                post => post.UserAccountId,
                user => user.UserAccountId,
                (post,user)=>new PostDTO
                {
                    UserAccountId = post.UserAccountId,
                    PostId = post.PostId,
                    TextContent = post.TextContent,
                    TimeStamp = post.TimeStamp,
                    PostLocation = post.PostLocation,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    PostPhotoPath = post.PostPhotoPath,
                    ProfilePhotoPath = user.ProfilePhotoPath
                })

            .Where(u => friendsList.Contains(u.UserAccountId))
            .OrderByDescending(u => u.TimeStamp)
            .ToList();

            foreach(var post in allPosts)
            {
                var isLiked = this.Context.Likes
                    .Where(l => l.PostId.Equals(post.PostId) && l.UserAccountId.Equals(userAccountId))
                    .FirstOrDefault();

                if (isLiked != null)
                {
                    post.isLiked = true;
                }
                else
                {
                    post.isLiked = false;
                }
            }

            return allPosts;
        }

        public IEnumerable<PostDTO> GetTimelinePosts(Guid userAccountId, string sessionId, string username)
        {
            var timeListPosts = this.Context.Posts.Join(
                this.Context.UserAccounts,
                post => post.UserAccountId,
                user => user.UserAccountId,
                (post, user) => new PostDTO
                {
                    UserAccountId = post.UserAccountId,
                    PostId = post.PostId,
                    TextContent = post.TextContent,
                    TimeStamp = post.TimeStamp,
                    PostLocation = post.PostLocation,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    PostPhotoPath = post.PostPhotoPath,
                    ProfilePhotoPath = user.ProfilePhotoPath,
                })

            .Where(u => u.PostLocation.Equals(userAccountId))
            .OrderByDescending(u => u.TimeStamp)
            .ToList();

            foreach (var post in timeListPosts)
            {
                var isLiked = this.Context.Likes
                    .Where(l => l.PostId.Equals(post.PostId) && l.UserAccountId.Equals(Guid.Parse(sessionId)))
                    .FirstOrDefault();

                if (isLiked != null)
                {
                    post.isLiked = true;
                }
                else
                {
                    post.isLiked = false;
                }
            }

            return timeListPosts;
        }

        public Guid GetUserAccountIdByPost(Guid postId)
        {
            var accountId = this.Context.Posts
                .Select(e => new { e.PostId, e.UserAccountId })
                .Where(u => u.PostId == postId)
                .FirstOrDefault();

            return accountId.UserAccountId;
        }
    }
}
