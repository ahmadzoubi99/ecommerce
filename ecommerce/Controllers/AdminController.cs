using ecommerce.Models;
using Ecommerce.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyContext _context;
        public AdminController(MyContext myContext)
        {
            _context = myContext;
        }
        public IActionResult Index()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.image = HttpContext.Session.GetString("image");

            // Calculate the total sales for all time
            var totalSale = _context.Orders.Sum(p => p.TotalAmount);
            ViewBag.totalSale = totalSale;

			ViewBag.productsCount = _context.Products.Count();
			ViewBag.userCount= _context.Users.Count();
             
            // Calculate the total sales for the last day
            var yesterday = DateTime.Now.Date.AddDays(-1);
            var today = DateTime.Now.Date;
            var lastDaySales = _context.Orders
                                       .Where(p => p.CreatedAt >= yesterday && p.CreatedAt < today)
                                       .Sum(p => (decimal?)p.TotalAmount) ?? 0;
            ViewBag.lastDaySales = lastDaySales;

            return View();
        }
        public async Task<IActionResult> Product()
        {
            var myContext = _context.Products.Include(p => p.Category);
            return View(await myContext.ToListAsync());
        }
        public async Task<IActionResult> Category()
        {
            var myContext = _context.Categories;
            return View(await myContext.ToListAsync());
        }
		public async Task<IActionResult> Order()
		{
			var myContext = _context.OrderItems
									.Include(oi => oi.Order)         // Include the Order entity related to OrderItem
										.ThenInclude(o => o.User)   // Include the User entity related to Order
									.Include(oi => oi.Product)       // Include the Product entity related to OrderItem
									.ToListAsync();                  // Execute the query

			var orderItems = await myContext;

			return View(orderItems);
		}
	

	}
}
