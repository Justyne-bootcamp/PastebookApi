using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
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
        [Route("/Index")]
        public async Task<IActionResult> Index()
        {
            var album = await _albumService.FindAll();
            return StatusCode(StatusCodes.Status200OK, album);
        }

        [HttpPost]
        [Route("insert/{albumName}")]
        public async Task<IActionResult> Insert(string albumName)
        {
            var userAccountId = HttpContext.Session.GetString("userAccountId");
            var album = new Album()
            {
                AlbumId = Guid.NewGuid(),
                AlbumName = albumName,
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
        [Route("delete/{albumId:Guid}")]
        public async Task<IActionResult> Delete(Guid albumId)
        {

            var album = await _albumService.Delete(albumId);
            return StatusCode(StatusCodes.Status200OK, album);
        }
    } 
}
