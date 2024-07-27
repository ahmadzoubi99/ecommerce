using ecommerce.Models;
using Ecommerce.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class UserProfileController : Controller
{
    private readonly MyContext _context;

    public UserProfileController(MyContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        int? userId = HttpContext.Session.GetInt32("userId");
        if (userId == null || _context.Users == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
  
    public async Task<IActionResult> Edit([Bind("Id,Username,Email,FullName,ImagePath,RoleId,Birthday,ImageFile,phoneNumber,Location")] User user, string newPassword)
    {
        int? userId = HttpContext.Session.GetInt32("userId");

        if (userId == null)
        {
            return RedirectToAction("Login", "Account"); // Redirect to login page or an appropriate action
        }

        user.Id = userId.Value;

        var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == user.Id);

        if (existingUser == null)
        {
            return NotFound();
        }

        // Retain existing values if not provided in the form
        if (string.IsNullOrEmpty(user.Username))
        {
            user.Username = existingUser.Username;
        }
        if (string.IsNullOrEmpty(newPassword))
        {
            user.PasswordHash = existingUser.PasswordHash;
        }
        else
        {
            // Hash the new password
            user.PasswordHash = HashPassword(newPassword);
        }
        if (string.IsNullOrEmpty(user.Email))
        {
            user.Email = existingUser.Email;
        }
        if (string.IsNullOrEmpty(user.FullName))
        {
            user.FullName = existingUser.FullName;
        }
        if (string.IsNullOrEmpty(user.ImagePath))
        {
            user.ImagePath = existingUser.ImagePath;
        }
        if (user.RoleId == null || user.RoleId == 0)
        {
            user.RoleId = existingUser.RoleId;
        }
        if (user.Birthday == null)
        {
            user.Birthday = existingUser.Birthday;
        }
        if (string.IsNullOrEmpty(user.phoneNumber))
        {
            user.phoneNumber = existingUser.phoneNumber;
        }
        if (string.IsNullOrEmpty(user.Location))
        {
            user.Location = existingUser.Location;
        }

        // Handle the file upload for ImageFile
        if (user.ImageFile != null)
        {
            var fileName = Path.GetFileName(user.ImageFile.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await user.ImageFile.CopyToAsync(fileStream);
            }
            user.ImagePath = "/images/" + fileName;
        }
        else
        {
            user.ImagePath = existingUser.ImagePath;
        }

        try
        {
            _context.Entry(existingUser).State = EntityState.Detached; // Detach existing tracked entity
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
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

        ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
        return View(user);
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }

    private string HashPassword(string password)
    {
        // Implement your password hashing logic here
        return password; // Replace this with actual hashing logic
    }
}
