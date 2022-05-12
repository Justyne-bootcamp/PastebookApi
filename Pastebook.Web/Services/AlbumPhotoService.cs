using Microsoft.AspNetCore.Http;
using Pastebook.Data.Models;
using Pastebook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pastebook.Web.Services
{
    public interface IAlbumPhotoService
    {
        public Task<IEnumerable<AlbumPhoto>> FindAll();
        public Task<AlbumPhoto> FindById(Guid albumPhotoId);
        public Task<AlbumPhoto> Delete(Guid albumPhotoId);
        public Task<AlbumPhoto> Insert(AlbumPhoto albumPhoto);
        public Task<AlbumPhoto> SystemPhotoDelete(Guid albumPhotoId);
    }
    public class AlbumPhotoService : IAlbumPhotoService
    {
        private IAlbumPhotoRepository _albumPhotoRepository;
        
        public AlbumPhotoService(IAlbumPhotoRepository albumPhotoRepository)
        {
            _albumPhotoRepository = albumPhotoRepository;
        }

        public async Task<IEnumerable<AlbumPhoto>> FindAll()
        {
            return await _albumPhotoRepository.FindAll();
        }

        public async Task<AlbumPhoto> FindById(Guid albumPhotoId)
        {
            return await _albumPhotoRepository.FindByPrimaryKey(albumPhotoId);
        }

        public async Task<AlbumPhoto> Delete(Guid albumPhotoId)
        {
            return await _albumPhotoRepository.Delete(albumPhotoId);
        }

        public class AllowedExtensionsAttribute: ValidationAttribute
        {
            private readonly string[] _extensions;
            public AllowedExtensionsAttribute(string[] extensions)
            {
                _extensions = extensions;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var file = value as IFormFile;
                if(file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
                return ValidationResult.Success;
            }

            public string GetErrorMessage()
            {
                return $"This photo extension is not allowed.";
            }
        }

        public async Task<AlbumPhoto> Insert(AlbumPhoto albumPhoto)
        {
            return await _albumPhotoRepository.Insert(albumPhoto);
        }

        public async Task<AlbumPhoto> SystemPhotoDelete(Guid albumPhotoId)
        {
            return await _albumPhotoRepository.SystemPhotoDelete(albumPhotoId);
        }
    }
}
