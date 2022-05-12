using Pastebook.Data.Data;
using Pastebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Repositories
{
    public interface IAlbumPhotoRepository : IBaseRepository<AlbumPhoto>
    {
    }
    public class AlbumPhotoRepository : GenericRepository<AlbumPhoto>, IAlbumPhotoRepository
    {

        public AlbumPhotoRepository(PastebookContext context) : base(context)
        {

        }

    }
}
