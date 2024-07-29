﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Context;
using ecommerce.Models;

namespace ecommerce.Controllers
{
    public class UsersController : Controller
    {
		
		private readonly MyContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UsersController(MyContext context, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {

            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.image = HttpContext.Session.GetString("image");
            var myContext = _context.Users.Include(u => u.Role);
            return View(await myContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {

            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.image = HttpContext.Session.GetString("image");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,PasswordHash,Email,FullName,Location,phoneNumber,ImagePath,RoleId,Birthday,ImageFile")] User user)
        {
            if (user.ImageFile != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;

                string fileName = Guid.NewGuid().ToString() + user.ImageFile.FileName;

                string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(fileStream);
                }

                user.ImagePath = fileName;
            }


            _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,PasswordHash,Email,FullName,Location,phoneNumber,ImagePath,RoleId,Birthday,ImageFile")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

         
                try
                {
                if (user.ImageFile != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;

                    string fileName = Guid.NewGuid().ToString() + user.ImageFile.FileName;

                    string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user.ImageFile.CopyToAsync(fileStream);
                    }

                    user.ImagePath = fileName;
                }
                _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'MyContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
