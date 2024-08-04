using ecommerce.Models;
using Ecommerce.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class authenticationController : Controller
    {
        private readonly MyContext _context;
        public authenticationController(MyContext myContext)
        {
            _context = myContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Id,Username,PasswordHash,Email,FullName,ImagePath,RoleId,Birthday")] User user)
        {
            
			var existingUser = _context.Users.Where(u => u.Email == user.Email && u.PasswordHash == user.PasswordHash).
                Include(p=>p.Role).FirstOrDefault();

            if (existingUser != null)
            {
                HttpContext.Session.SetInt32("userId", existingUser.Id);
				HttpContext.Session.SetInt32("RoleId", existingUser.Role.Id);

				HttpContext.Session.SetString("name", existingUser.FullName);
                HttpContext.Session.SetString("image", existingUser.ImagePath);

                int x =Convert.ToInt32(HttpContext.Session.GetInt32("userId"));
                if (HttpContext.Session.GetInt32("countOfItem") > 0)
                {
                    return RedirectToAction("Index", "Cart");
                }
                

                switch (existingUser.RoleId)
                {
                    case 1:
                        return RedirectToAction("Index", "Admin");
                    case 2:

                        return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.LoginError = "username or password is incoorect pleace try again .";
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register
            ([Bind("Id,Username,PasswordHash,Email,FullName,ImagePath,RoleId,Birthday,Location,phoneNumber")] User user)
        {
<<<<<<< HEAD
            user.Username = user.Username;
            user.Email= user.Email;
            user.ImagePath = "";
            user.FullName=user.Username;
            user.PasswordHash = user.PasswordHash;
            user.Birthday= user.Birthday;
            user.Location = user.Location;
            user.phoneNumber = user.phoneNumber;
            user.RoleId = 2;
            var existingUser = _context.Users
                .Where(u => u.Email == user.Email)
                .FirstOrDefault();
=======
            if (string.IsNullOrEmpty(user.Username))
            {
                ViewBag.RegisterError = "Enter your username";
                return View("Login", user);
            }
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                ViewBag.RegisterError = "Enter your password";
                return View("Login", user);
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                ViewBag.RegisterError = "Enter your email";
                return View("Login", user);
            }
            if (user.Birthday == null)
            {
                ViewBag.RegisterError = "Enter your birthday";
                return View("Login", user);
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
>>>>>>> 40f774a5c2b383cd629d429fd8aeceafe096b023

            if (existingUser != null)
            {
                ViewBag.RegisterError = "Email is already used";
                return View("Login", user);
            }
            else
            {
<<<<<<< HEAD
                _context.Add(user);
                await _context.SaveChangesAsync();
                // Redirect to the login page or another appropriate action
                return RedirectToAction("Login");
=======
                user.RoleId = 2;
                user.FullName = user.Username;
                user.ImagePath = " ";
                _context.Add(user);
                await _context.SaveChangesAsync();
                ViewBag.Success = true;
                ViewBag.Message = "Registration Successful!";
                return View("Login");
>>>>>>> 40f774a5c2b383cd629d429fd8aeceafe096b023
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "authentication");
        }


    }
}
