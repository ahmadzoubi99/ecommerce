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
