using FlowersTask.DAL;
using FlowersTask.Helper;
using FlowersTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Data;

namespace FlowersTask.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("manage")]
    public class FlowerController : Controller
    {
        private readonly FlowersDbContext _context;
        private readonly IWebHostEnvironment _env;
        public FlowerController(FlowersDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var data = _context.Flowers.Include(x => x.FlowerCatagories).ThenInclude(x => x.Catagory).Include(x => x.Images.Where(y => y.IsMain == true)).ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            ViewBag.Genres = _context.Catagories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Flower flower)
        {
            ViewBag.Genres = _context.Catagories.ToList();

            if (flower.MainImage == null)
            {
                ModelState.AddModelError("MainImage", "Main Image is required!");
            }
            if (_context.Flowers.Any(x => x.Name == flower.Name))
            {
                ModelState.AddModelError("Name", "Already have this name!");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var ids=flower.CategoryIds;
            foreach (var item in ids)
            {
                var flowercat = new FlowerCatagory()
                {
                    CatagoryId=item,
                    Flower=flower
                };
                _context.FlowerCatagory.Add(flowercat);
            }
            _context.Flowers.Add(flower);
            var mainImage = new Image()
            {
                ImageName = FileManager.AddFile(_env.WebRootPath, "manage/upload/flower", flower.MainImage),
                IsMain = true,
                Flower = flower,
            };
            foreach (IFormFile item in flower.OtherImages)
            {
                var image = new Image()
                {
                    ImageName = FileManager.AddFile(_env.WebRootPath, "manage/upload/flower", item),
                    IsMain = false,
                    Flower = flower,
                };
                _context.Images.Add(image);
            }

            _context.Images.Add(mainImage);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Genres = _context.Catagories.ToList();

            var flower = _context.Flowers.Include(x => x.Images).Include(x => x.FlowerCatagories).ThenInclude(x => x.Catagory).FirstOrDefault(x => x.Id == id);
            if (flower == null) { return View(); }
            flower.CategoryIds=flower.FlowerCatagories.Select(x=>x.CatagoryId).ToList();
            return View(flower);
        }
        [HttpPost]
        public IActionResult Edit(Flower flower)
        {
            ViewBag.Genres = _context.Catagories.ToList();
            var mainflower = _context.Flowers.Include(x => x.Images).Include(x => x.FlowerCatagories).ThenInclude(x => x.Catagory).FirstOrDefault(x => x.Id == flower.Id);

            List<Flower> sameflowers = _context.Flowers.Where(x => x.Name == flower.Name).ToList();
            foreach (Flower item in sameflowers)
            {
                if (item.Id != mainflower.Id)
                {
                    ModelState.AddModelError("Name", "Already have this name!");
                }
            }
            if (!ModelState.IsValid)
            {
                return View(mainflower);
            }

            mainflower.Name = flower.Name;
            mainflower.Price = flower.Price;
            mainflower.Description = flower.Description;
            foreach (var item in mainflower.FlowerCatagories.ToList())
            {
                _context.FlowerCatagory.Remove(item);
            }
            foreach (var item in flower.CategoryIds)
            {
                var flowercat = new FlowerCatagory()
                {
                    Catagory = _context.Catagories.Find(item)
                };
                mainflower.FlowerCatagories.Add(flowercat);
            }
            List<string> deleteImage = new List<string>();
            if (flower.MainImage != null)
            {
                var mainimg = mainflower.Images.Find(x => x.IsMain == true);
                deleteImage.Add(mainimg.ImageName);
                Image image = new Image()
                {
                    ImageName = FileManager.AddFile(_env.WebRootPath, "manage/upload/flower", flower.MainImage),
                    IsMain = true,
                    Flower = mainflower
                };
                _context.Remove(mainimg);
                _context.Images.Add(image);
            }
            var cont = mainflower.Images.Where(x => x.IsMain == false).Select(x => x.Id).ToList();
            foreach (var item in cont)
            {
                if (!flower.ImagesIds.Contains(item))
                {
                    var removeimg = _context.Images.Find(item);
                    deleteImage.Add(removeimg.ImageName);
                    _context.Images.Remove(removeimg);
                }
            }
            foreach (IFormFile item in flower.OtherImages)
            {
                Image image = new Image()
                {
                    ImageName = FileManager.AddFile(_env.WebRootPath, "manage/upload/flower", item),
                    IsMain = false,
                    Flower = mainflower
                };
                mainflower.Images.Add(image);
            }


            _context.SaveChanges();
            foreach (var item in deleteImage)
            {
                FileManager.Delete(_env.WebRootPath, "manage/upload/flower", item);
            }

            return RedirectToAction("index");

        }
        public IActionResult Delete(int id)
        {
            var data = _context.Flowers.Include(x => x.Images).FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return View("error");
            }
            List<string> rmvImg = new List<string>();
            foreach (var item in data.Images)
            {
                rmvImg.Add(item.ImageName);
            }
            _context.Remove(data);
            _context.SaveChanges();
            foreach (var item in rmvImg)
            {
                FileManager.Delete(_env.WebRootPath, "manage/upload/flower", item);
            }
            return RedirectToAction("index");
        }
    }
}
