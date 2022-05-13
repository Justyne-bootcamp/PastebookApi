using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
using Pastebook.Web.DataTransferObjects;
using Pastebook.Web.Http;
using Pastebook.Web.Models;
using Pastebook.Web.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pastebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AlbumPhotoController : ControllerBase
    {
        
        private readonly IAlbumPhotoService _albumPhotoService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        

        public AlbumPhotoController(IAlbumPhotoService albumPhotoService, IWebHostEnvironment webHostEnvironment)
        {
            _albumPhotoService = albumPhotoService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var albumPhotos = await _albumPhotoService.FindAll();
            return StatusCode(StatusCodes.Status200OK, albumPhotos);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] AlbumPhotoFormDTO albumPhotoForm)
        {
            try
            {
                var fileName = albumPhotoForm.Photo.files.FileName;
                FileInfo file = new FileInfo(fileName);
                var ext = file.Extension;
                var albumId = albumPhotoForm.AlbumId;
                var albumName = albumPhotoForm.AlbumName;
                var userName = HttpContext.Session.GetString("username");
                
                if(albumPhotoForm.Photo != null)
                {
                    if (albumPhotoForm.Photo.files.Length > 0)
                    {
                        string path = $@"{_webHostEnvironment.WebRootPath}\{userName}\{albumName}\";

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        
                        using (FileStream fileStream = System.IO.File.Create(path + DateTime.Now.ToString("yyyyMMddhhmmss") + ext))
                        {
                            albumPhotoForm.Photo.files.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                        var albumPhoto = new AlbumPhoto()
                        {
                            AlbumPhotoId = Guid.NewGuid(),
                            AlbumId = albumId,
                            AlbumPhotoPath = $@"{userName}\{albumName}\{DateTime.Now.ToString("yyyyMMddhhmmss")+ ext}"
                        };
                        var newAlbumPhoto = await _albumPhotoService.Insert(albumPhoto);
                        return StatusCode(StatusCodes.Status200OK, newAlbumPhoto);
                    }
                    
                }
                return StatusCode(
                            StatusCodes.Status400BadRequest,
                            new HttpResponseError()
                            {
                                Message = "No Image Found.",
                                StatusCode = StatusCodes.Status400BadRequest
                            });

            }
            catch (Exception ex)
            {
                return StatusCode(
                        StatusCodes.Status400BadRequest,
                        new HttpResponseError()
                        {
                            Message = "No Image Found.",
                            StatusCode = StatusCodes.Status400BadRequest
                        });
            }
        }

        [HttpDelete]
        [Route("delete/{albumPhotoId:Guid}")]
        public async Task<IActionResult> Delete(Guid albumPhotoId)
        {
            var deleteAlbumPhoto = await _albumPhotoService.SystemPhotoDelete(albumPhotoId);
            var albumPhoto = await _albumPhotoService.Delete(albumPhotoId);
            return StatusCode(StatusCodes.Status200OK, albumPhoto);
        }
    }
}
