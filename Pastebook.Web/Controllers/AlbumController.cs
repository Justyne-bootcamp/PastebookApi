using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
using Pastebook.Web.DataTransferObjects;
using Pastebook.Web.Http;
using Pastebook.Web.Services;
using System;
using System.Threading.Tasks;

namespace Pastebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly IAlbumPhotoService _albumPhotoService;

        public AlbumController(IAlbumService albumService, IAlbumPhotoService albumPhotoService)
        {
            _albumService = albumService;
            _albumPhotoService = albumPhotoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var album = await _albumService.FindAll();
            return StatusCode(StatusCodes.Status200OK, album);
        }

        [HttpGet]
        [Route("myalbum/{userAccountId}")]
        public IActionResult GetMyAlbums(string userAccountId)
        {
            var albums = _albumService.GetAlbumByUserAccountId(Guid.Parse(userAccountId));
            return StatusCode(StatusCodes.Status200OK, albums);
        }

        [HttpPost]
        [Route("insert")]
        public async Task<IActionResult> Insert([FromBody] AlbumFormDTO albumForm)
        {
            var album = new Album()
            {
                AlbumId = Guid.NewGuid(),
                AlbumName = albumForm.AlbumName,
                UserAccountId = Guid.Parse(albumForm.UserAccountId)
            };

            var newAlbum = await _albumService.Insert(album);
            if(newAlbum != null)
            {
                return StatusCode(StatusCodes.Status201Created, newAlbum);
            }
            else
            {
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new HttpResponseError()
                    {
                        Message = "Album not Created",
                        StatusCode = StatusCodes.Status404NotFound
                    });
            }
        }
        [HttpDelete]
        [Route("delete/{albumId}")]
        public async Task<IActionResult> Delete(string albumId)
        {
            var album = await _albumService.Delete(Guid.Parse(albumId));
            _albumPhotoService.DeletePhotosByAlbumId(Guid.Parse(albumId));
            return StatusCode(StatusCodes.Status200OK, album);
        }
    } 
}
