using ecommerce.Models;
using Ecommerce.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Controllers
{
	public class HomeController : Controller
	{
		private readonly MyContext myContext;

		public HomeController(MyContext myContext)
		{
			this.myContext = myContext;
		}


        public async Task<IActionResult> Index()
		{
			var products = myContext.Products.Include(c => c.Category).ToList();
			var categories = myContext.Categories.ToList();
			var testimonials = myContext.Testimonials.Include(u => u.User).Where(t => t.Status == "Approved").ToList();
			var model3 = Tuple.Create<IEnumerable<Category>, IEnumerable<Product>, IEnumerable<Testimonial>>(categories, products,testimonials);
			if (HttpContext.Session.GetInt32("userId") != null)
			{
				ViewBag.Login="Login";
			}
			
			return View("Index", model3);
		}


		public async Task<IActionResult> Shop()
		{
			var count= HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count =HttpContext.Session.GetInt32("countOfItem");

            var products = myContext.Products.ToList();
			var categories = myContext.Categories.ToList();
			if (HttpContext.Session.GetInt32("userId") != null)
			{
				ViewBag.Login = "Login";
			}

			var model3 = Tuple.Create<IEnumerable<Category>, IEnumerable<Product>>(categories, products);


			return View(model3);
		}

		[HttpPost]
		[HttpPost]
		public async Task<IActionResult> SearchByProudctName(string? name)
		{
			// Start with the base query for categories
			var modelContext = myContext.Products.AsQueryable();

			// Add filter conditionally if 'name' is provided
			if (!string.IsNullOrEmpty(name))
			{
				modelContext = modelContext.Where(c => c.Name.Contains(name));
			}

			// Execute the query and get the filtered categories
			var products = await modelContext.ToListAsync();

			// Retrieve the list of products (not filtered in this case)
			var categories = await myContext.Categories.ToListAsync();

			// Combine categories and products into a tuple
			var model = Tuple.Create<IEnumerable<Category>, IEnumerable<Product>>(categories, products);

			// Return the Shop view with the combined model
			return View("Shop", model);
		}
		[HttpPost]
        //public async Task<IActionResult> ProductByCategorie(int categoryId)
        //{
        //	var products = await myContext.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
        //	var categories = await myContext.Categories.ToListAsync();

        //	var model = Tuple.Create<IEnumerable<Category>, IEnumerable<Product>>(categories, products);

        //	return View("Shop", model);
        //}
/*        [HttpPost]
		public async Task<IActionResult> ProductByCategorie(int categoryId)
		{
			var products = await myContext.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
			return PartialView("_ProductListPartial", products);
		}
*/

		public async Task<IActionResult> ProductByCategorie(int id)
		{
			var products = await myContext.Products.Where(p => p.CategoryId == id).ToListAsync();
			var categories = await myContext.Categories.ToListAsync();

			var model = Tuple.Create<IEnumerable<Category>, IEnumerable<Product>>(categories, products);

			return View("Shop", model);
		}

		public IActionResult ContactUs()
		{
			if (HttpContext.Session.GetInt32("userId") != null)
			{
				ViewBag.Login = "Login";
			}
			return View();
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs([Bind("Id,Name,Email,Subject,Message,UserId")] ContactUs contactUs)
        {
			contactUs.Subject = "";
			if (HttpContext.Session.GetInt32("userId") != null)
			{
				ViewBag.Login = "Login";
			}
			contactUs.UserId = HttpContext.Session.GetInt32("userId");
            myContext.Add(contactUs);
            await myContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["UserId"] = new SelectList(myContext.Users, "Id", "Id", contactUs.UserId);
            return View(contactUs);
        }
        
        public IActionResult AboutUs()
		{
			if (HttpContext.Session.GetInt32("userId") != null)
			{
				ViewBag.Login = "Login";
			}
			return View();
		}
		public IActionResult Cart()
		{
			if (HttpContext.Session.GetInt32("userId") != null)
			{
				ViewBag.Login = "Login";
			}
			return View();
		}

		[HttpPost]
		public IActionResult Cart(int productId)
		{

			return View();
		}
        // GET: Testimonials/Create
        public IActionResult Create()
        {
			if (HttpContext.Session.GetInt32("userId") != null)
			{
				ViewBag.Login = "Login";
			}
			ViewData["UserId"] = new SelectList(myContext.Users, "Id", "Id");
            return View();
        }
		public IActionResult CTestimonial() 
		{
			if (HttpContext.Session.GetInt32("userId") != null)
			{
				ViewBag.Login = "Login";
			}
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CTestimonial([Bind("Id,UserId,Content,Status")] Testimonial testimonial)
        {
            var userId = HttpContext.Session.GetInt32("userId");

			if (userId == null)
			{
				return RedirectToAction("Login", "authentication");
			}
            testimonial.UserId = userId.Value; 
            testimonial.Status = "Pending";

            
                myContext.Add(testimonial);
                await myContext.SaveChangesAsync();

                // Redirect to the Index action
                return RedirectToAction(nameof(Index));
           

            // If the model state is not valid, set the ViewData and return the view
            ViewData["UserId"] = new SelectList(myContext.Users, "Id", "Id", testimonial.UserId);
            return View(testimonial);
        }


    }
}
