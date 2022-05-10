using Pastebook.Data.Models;
using Pastebook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pastebook.Web.Services
{
    public interface IAlbumService
    {
        public Task<IEnumerable<Album>> FindAll();
        public Task<Album> FindById(Guid id);

        public Task<Album> Insert(Album album);
        public Task<Album> Delete(Guid albumId);

    }
    public class AlbumService : IAlbumService
    {
        private IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }
        public async Task<IEnumerable<Album>> FindAll()
        {
            return await _albumRepository.FindAll();
        }

        public async Task<Album> FindById(Guid id)
        {
            return await _albumRepository.FindByPrimaryKey(id);
        }

        public async Task<Album> Insert(Album album)
        {
            return await _albumRepository.Insert(album);
        }

        public async Task<Album> Delete(Guid albumId)
        {
            return await _albumRepository.Delete(albumId);
        }

    }
}
