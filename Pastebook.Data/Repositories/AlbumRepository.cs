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
        public IEnumerable<Album> GetAlbumByUsername(string username);
    }
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {

        public AlbumRepository(PastebookContext context) : base(context)
        {

        }

        public IEnumerable<Album> GetAlbumByUsername(string username)
        {
            var user = this.Context.UserAccounts
                .Select(e => e)
                .Where(e => e.Username.Equals(username))
                .FirstOrDefault();

            var albums = this.Context.Albums
                .Select(e => e)
                .Where(e => e.UserAccountId.Equals(user.UserAccountId))
                .ToList();

            return albums;
        }
    }
}
