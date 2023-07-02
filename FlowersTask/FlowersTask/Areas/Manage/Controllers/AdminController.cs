using FlowersTask.Areas.Manage.ViewModels;
using FlowersTask.DAL;
using FlowersTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Mime;

namespace FlowersTask.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;
        private readonly FlowersDbContext _context;
        public AdminController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager, SignInManager<AppUser> singInManager,FlowersDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _singInManager = singInManager;
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginVM admin)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var mainadmin= await _userManager.FindByNameAsync(admin.UserName);
            if (mainadmin==null)
            {
                ModelState.AddModelError("", "Username or Password is incorract!");
                return View();
            }
            var result = await _singInManager.PasswordSignInAsync(mainadmin, admin.Password,false,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is incorract!");
                return View();
            }
            return RedirectToAction("index","dashboard");
        }


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Register(AdminRegisterVM admin)
        {
            if (!ModelState.IsValid) { return View(); }
            var data = new AppUser()
            {
                FullName=admin.FullName,
                UserName=admin.Username,
                Email=admin.EmailAddress
            };
             var result= await _userManager.CreateAsync(data,admin.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(data,"Admin");
            return RedirectToAction("Login");
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Roles()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            List<AppUser> datas= _context.AppUsers.ToList();
            return View(datas);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(string id) 
        {
            ViewBag.Roles= _roleManager.Roles.ToList();
            List<string> rolesIds=new List<string>();
                
               rolesIds= _context.UserRoles.Where(x=>x.UserId==id).Select(x=>x.RoleId).ToList();
            List<string> roleNames=new List<string>();
            foreach (var item in rolesIds)
            {
                var data = _context.Roles.Find(item).Name;
                roleNames.Add(data);
            }
            ViewBag.UserRoles = roleNames;
            var admin= await _userManager.FindByIdAsync(id);
            if (admin==null) return View("Error");
            return View(admin);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Edit(AppUser admin)
        {
            var mainadmin=await _userManager.FindByIdAsync(admin.Id);
            if (!ModelState.IsValid)
            {
                return View(mainadmin);
            }
            mainadmin.FullName= admin.FullName;
            mainadmin.UserName= admin.UserName;
            mainadmin.Email= admin.Email;

            var userRoles=_context.UserRoles.Where(x => x.UserId == mainadmin.Id).ToList();
            if (admin.RolesIds.Count>0)
            {
                foreach (var item in userRoles)
                {
                    _context.UserRoles.Remove(item);
                }
            }
            _context.SaveChanges();

            foreach (var item in admin.RolesIds)
            {
                var role = await _roleManager.FindByIdAsync(item);
                await _userManager.AddToRoleAsync(mainadmin, role.Name);
            }   
            _context.SaveChanges();
            return RedirectToAction("Roles");
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser admin=await _userManager.FindByIdAsync(id);
            if (admin == null) return View("error");
            await _userManager.DeleteAsync(admin);
            _context.SaveChanges();

            return RedirectToAction("roles");
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            ViewBag.Roles = _context.Roles.ToList();
                return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create(AdminRegisterVM admin)
        {
            ViewBag.Roles=_context.Roles.ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser newadmin= new AppUser()
            {
                FullName= admin.FullName,
                UserName=admin.Username,
                Email=admin.EmailAddress
            };
            var result= await _userManager.CreateAsync(newadmin,admin.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            foreach (var item in admin.RolesIds)
            {
                var roleName = _context.Roles.Find(item).Name;
                await _userManager.AddToRoleAsync(newadmin, roleName);
            }
            return RedirectToAction("roles");
        }
    }


}
