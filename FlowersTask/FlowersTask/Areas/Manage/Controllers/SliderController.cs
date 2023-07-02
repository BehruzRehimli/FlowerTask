using FlowersTask.DAL;
using Microsoft.AspNetCore.Mvc;
using FlowersTask.Models;
using FlowersTask.Helper;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace FlowersTask.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("manage")]
    public class SliderController : Controller
    {
        private readonly FlowersDbContext _context;

        private IWebHostEnvironment _env { get; }

        public SliderController(FlowersDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {

            return View(_context.Sliders.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid) { return View(); }
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("File", "File cant be requried!");
                return View();
            }
            slider.Image = FileManager.AddFile(_env.WebRootPath, "manage/upload/slider", slider.ImageFile);
            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null)
            {
                return View("error");
            }
            return View(slider);
        }
        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            Slider mainSlider = _context.Sliders.Find(slider.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (mainSlider == null)
            {
                return View("error");
            }
            mainSlider.Title1 = slider.Title1;
            mainSlider.Title2 = slider.Title2;
            mainSlider.Desc = slider.Desc;
            mainSlider.Signature = slider.Signature;
            mainSlider.OrderSlider = slider.OrderSlider;
            var deleteFile = mainSlider.Image;
            if (slider.ImageFile != null)
            {
                mainSlider.Image = FileManager.AddFile(_env.WebRootPath, "manage/upload/slider", slider.ImageFile);
            }

            _context.SaveChanges();
            FileManager.Delete(_env.WebRootPath, "manage/upload/slider", deleteFile);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);
            if (slider == null)
            {
                return View("error");
            }
            var deleteFile = slider.Image;
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            FileManager.Delete(_env.WebRootPath, "manage/upload/slider", deleteFile);
            return RedirectToAction("Index");
        }
    }
}
