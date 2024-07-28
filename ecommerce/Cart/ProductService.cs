using ecommerce.Models;
using Ecommerce.Context;

namespace ecommerce.Cart
{
    public class ProductService:IProductService
    {
        private readonly MyContext _context;

        public ProductService(MyContext context)
        {
            _context = context;
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

    }
}
