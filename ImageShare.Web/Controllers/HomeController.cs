using ImageShare.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ImageShare.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;

namespace ImageShare.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _environment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _connectionString = configuration.GetConnectionString("conStr");
            _environment = environment;
        }

        public IActionResult Index()
        {

            var repo = new ImagesRepository(_connectionString);
            var vm = new HomeViewModel
            {
                Images = repo.GetImages()
            };
            return View(vm);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(Image image, IFormFile imageFile)
        {
            string actualFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string finalFileName = Path.Combine(_environment.WebRootPath, "uploads", actualFileName);
            using (FileStream fs = new FileStream(finalFileName, FileMode.CreateNew))
            {
                imageFile.CopyTo(fs);
            }

            image.FileName = actualFileName;

            var repo = new ImagesRepository(_connectionString);
            repo.Add(image);

            return Redirect("/");
        }

        [HttpPost]
        public IActionResult ViewImage(int? id)
        {
            var repo = new ImagesRepository(_connectionString);
            if (!id.HasValue)
            {
                return Redirect("/");
            }

            var image = repo.GetImageById(id.Value);
            if (image == null)
            {
                return Redirect("/");
            }

            var vm = new ImageViewModel { Image = image };

            if (HttpContext.Session.GetString("likedids") != null)
            {
                var likedids = HttpContext.Session.Get<List<int>>("likedids");
                vm.CanLike = likedids.All(i => i != id);
            }
            else
            {
                vm.CanLike = true;
            }
            return View(vm);


        }

        [HttpPost]
        public void LikeImage(int id)
        {
            var repo = new ImagesRepository(_connectionString);
            repo.AddLikes(id);
            List<int> likedIds = HttpContext.Session.Get<List<int>>("likedids") ?? new List<int>();
            likedIds.Add(id);
            HttpContext.Session.Set("likedids", likedIds);
        }
        public ActionResult GetLikes(int id)
        {
            var repo = new ImagesRepository(_connectionString);
            return Json(new { Likes = repo.GetLikes(id) });
        }
    }

        public static class SessionExtensions
        {
            public static void Set<T>(this ISession session, string key, T value)
            {
                session.SetString(key, JsonConvert.SerializeObject(value));
            }

            public static T Get<T>(this ISession session, string key)
            {
                string value = session.GetString(key);

                return value == null ? default(T) :
                    JsonConvert.DeserializeObject<T>(value);
            }
        }
}
