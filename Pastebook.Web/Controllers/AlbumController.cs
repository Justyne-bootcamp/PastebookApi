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

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var album = await _albumService.FindAll();
            return StatusCode(StatusCodes.Status200OK, album);
        }

        [HttpGet]
        [Route("myalbum")]
        public IActionResult GetMyAlbums()
        {
            Guid userAccountId = Guid.Parse("FF2E9BD8-37A7-4980-8FC2-43AE89BB7A8D");
            var albums = _albumService.GetAlbumByUserAccountId(userAccountId);
            return StatusCode(StatusCodes.Status200OK, albums);
        }

        [HttpPost]
        [Route("insert")]
        public async Task<IActionResult> Insert([FromBody] AlbumFormDTO albumForm)
        {
            //var userAccountId = HttpContext.Session.GetString("userAccountId");
            var userAccountId = "FF2E9BD8-37A7-4980-8FC2-43AE89BB7A8D";
            var album = new Album()
            {
                AlbumId = Guid.NewGuid(),
                AlbumName = albumForm.AlbumName,
                UserAccountId = Guid.Parse(userAccountId)
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
            return StatusCode(StatusCodes.Status200OK, album);
        }
    } 
}
