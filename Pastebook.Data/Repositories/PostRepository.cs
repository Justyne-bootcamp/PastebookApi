using Microsoft.EntityFrameworkCore;
using Pastebook.Data.Data;
using Pastebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Repositories
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        
    }
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(PastebookContext context) : base(context)
        {
            this.Context = context;
        }
    }
}
