using FlowersTask.DAL;
using FlowersTask.Helper;
using FlowersTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowersTask.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("manage")]
    public class CategoryController : Controller
    {
        private readonly FlowersDbContext _context;

        private IWebHostEnvironment _env { get; }

        public CategoryController(FlowersDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {

            return View(_context.Catagories.Include(x => x.FlowerCatagories).ThenInclude(x=>x.Flower).ToList());
        }
        public IActionResult Create()   
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Catagory genre)
        {
            if (_context.Catagories.Any(x=>x.Name==genre.Name))
            {
                ModelState.AddModelError("Name","Already have this name!");
            }
            if (!ModelState.IsValid) { return View(); }

            _context.Catagories.Add(genre);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            Catagory genre = _context.Catagories.FirstOrDefault(x => x.Id == id);
            if (genre == null)
            {
                return View("error");
            }
            return View(genre);
        }
        [HttpPost]
        public IActionResult Edit(Catagory genre)
        {
            Catagory mainGenre = _context.Catagories.Find(genre.Id);
            if (_context.Catagories.Any(x => x.Name == genre.Name && x.Id!=genre.Id))
            {
                ModelState.AddModelError("Name", "Already have this name!");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (mainGenre == null)
            {
                return View("error");
            }
            mainGenre.Name = genre.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Catagory catagory = _context.Catagories.Include(x=>x.FlowerCatagories).ThenInclude(x=>x.Flower).FirstOrDefault(x=>x.Id==id);
            if (catagory == null)
            {
                return View("error");
            }

            _context.Catagories.Remove(catagory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
