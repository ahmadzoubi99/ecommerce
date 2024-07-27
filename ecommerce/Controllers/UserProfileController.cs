
using ecommerce.Models;
using Ecommerce.Context;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;



public class UserProfileController : Controller
{
    private readonly MyContext _context;

    public UserProfileController(MyContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Username,PasswordHash,Email,FullName,ImagePath,RoleId,Birthday,ImageFile")] User user)
    {
        if (HttpContext.Session.GetInt32("UserId").ToString() != null)
        {
            var userId = HttpContext.Session.GetInt32("UserId").ToString();
            user.Id = Convert.ToInt32(userId);
            var userss = _context.Users.Where(p=>p.Id==user.Id).FirstOrDefault();
        }
        try
        {
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

            return RedirectToAction(nameof(Index));
        }
        ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
        return View(user);
    }

    private bool UserExists(int id)
    {
        throw new NotImplementedException();
    }
    public IActionResult Edit()
    {
        return View();
    }
}


//    public IActionResult Profile()
//    {
//        var username = User.Identity.Name;
//        var userProfile = _context.Users.SingleOrDefault(u => u.Username == username);

//        if (userProfile == null)
//        {
//            return NotFound();
//        }

//        return View(userProfile);
//    }
//}
//    [HttpGet]
//    public IActionResult Edit()
//    {
//        var username = User.Identity.Name;
//        var userProfile = _context.Users.SingleOrDefault(u => u.Username == username);

//        if (userProfile == null)
//        {
//            return NotFound();
//        }

//        return View(userProfile);
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Edit(User userProfile)
//    {
//        if (ModelState.IsValid)
//        {
//            var user = _context.Users.SingleOrDefault(u => u.Id == userProfile.Id);
//            if (user == null)
//            {
//                return NotFound();
//            }

//            user.Email = userProfile.Email;
//            user.FullName = userProfile.FullName;
//            user.ImagePath = userProfile.ImagePath;
//            user.Birthday = userProfile.Birthday;

//            await _context.SaveChangesAsync();
//            return RedirectToAction("Profile");
//        }

//        return View(userProfile);
//    }
//}
