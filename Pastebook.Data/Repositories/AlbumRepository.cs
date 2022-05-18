using Pastebook.Data.Data;
using Pastebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Pastebook.Data.Repositories
{
    public interface IAlbumRepository: IBaseRepository<Album>
    {
        public IEnumerable<Album> GetAlbumByUserAccountId(Guid userId);
    }
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {

        public AlbumRepository(PastebookContext context) : base(context)
        {

        }

        public IEnumerable<Album> GetAlbumByUserAccountId(Guid userId)
        {
            var album = this.Context.Albums
                .Select(e => e)
                .Where(e => e.UserAccountId.Equals(userId))
                .ToList();

            return album;
        }
    }
}
