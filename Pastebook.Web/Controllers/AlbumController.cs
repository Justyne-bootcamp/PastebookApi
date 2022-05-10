using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
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


        public AlbumController(IAlbumService albumService, IUserAccountService userAccountService)
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
        [Route("/Insert")]
        public IActionResult Insert()
        {
            var newAlbumId = Guid.NewGuid();
            var album = new Album()
            {
                AlbumId = newAlbumId,
                AlbumName = "someAlbum",
                UserAccountId = Guid.Parse("6BE600D8-E367-4E04-9EF1-72D9F138B151")
            };
            var newAlbum = _albumService.Insert(album);
            return StatusCode(StatusCodes.Status200OK, album);
        }
        [HttpDelete]
        [Route("/Delete")]
        public async Task<IActionResult> Delete()
        {

            var albumId = Guid.Parse("BA4F873D-3FCD-496E-B110-C1712D246D9F");
            var album = await _albumService.Delete(albumId);
            return StatusCode(StatusCodes.Status200OK, album);
        }
    } 
}
