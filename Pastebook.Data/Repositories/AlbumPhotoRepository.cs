using Pastebook.Data.Data;
using Pastebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Pastebook.Data.Repositories
{
    public interface IAlbumPhotoRepository : IBaseRepository<AlbumPhoto>
    {
        public Task<AlbumPhoto> SystemPhotoDelete(Guid albumPhotoId);
        public IEnumerable<AlbumPhoto> GetByAlbumId(Guid albumId);
        public void DeletePhotosByAlbumId(Guid albumId);
    }
    public class AlbumPhotoRepository : GenericRepository<AlbumPhoto>, IAlbumPhotoRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AlbumPhotoRepository(PastebookContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void DeletePhotosByAlbumId(Guid albumId)
        {
            var photos = this.Context.AlbumPhotos
                .Select(e => e)
                .Where(e => e.AlbumId.Equals(albumId))
                .ToList();
            this.Context.AlbumPhotos.RemoveRange(photos);
            this.Context.SaveChanges();
        }

        public IEnumerable<AlbumPhoto> GetByAlbumId(Guid albumId)
        {
           var photos = this.Context.AlbumPhotos
                .Select(e => e)
                .Where(p => p.AlbumId == albumId)
                .ToList();
            return photos;
        }

        public async Task<AlbumPhoto> SystemPhotoDelete(Guid albumPhotoId)
        {
            var deleteAlbumPhoto = await FindByPrimaryKey(albumPhotoId);
            if (deleteAlbumPhoto != null)
            {

                string filePath = $@"{_webHostEnvironment.WebRootPath}\{deleteAlbumPhoto.AlbumPhotoPath}";
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            return deleteAlbumPhoto;
        }
    }
}
