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
        public IEnumerable<PostDTO> GetAllNewsfeedPost(List<Guid> friendsList);
    }
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(PastebookContext context) : base(context)
        {
            this.Context = context;
        }

        public IEnumerable<PostDTO> GetAllNewsfeedPost(List<Guid> friendsList)
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
                    PostPhotoPath = post.PostPhotoPath,
                    ProfilePhotoPath = user.ProfilePhotoPath
                })

            .Where(u => friendsList.Contains(u.UserAccountId))
            .OrderByDescending(u => u.TimeStamp)
            .ToList();



            return allPosts;
        }
    }
}
