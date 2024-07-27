﻿using ecommerce.Models;
using Ecommerce.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var existingUser = _context.Users.Where(u => u.Email == user.Email && u.PasswordHash == user.PasswordHash).FirstOrDefault();

            if (existingUser != null)
            {
                HttpContext.Session.SetInt32("userId", existingUser.Id);
                int x =Convert.ToInt32(HttpContext.Session.GetInt32("userId"));
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
            ([Bind("Id,Username,PasswordHash,Email,FullName,ImagePath,RoleId,Birthday")] User user)
        {
            user.Email= user.Email;
            user.ImagePath = "";
            user.FullName=user.Username;
            user.PasswordHash = user.PasswordHash;
            user.Birthday= user.Birthday;
            user.RoleId = 2;
            var existingUser = _context.Users
                .Where(u => u.Email == user.Email)
                .FirstOrDefault();

            if (existingUser != null)
            {
                ViewBag.RegisterError = "Email is already used";
                return View("Login", user);
            }
            else
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                // Redirect to the login page or another appropriate action
                return RedirectToAction("Login");
            }
        }




        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


    }
}
