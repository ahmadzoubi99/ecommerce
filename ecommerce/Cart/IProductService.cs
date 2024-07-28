using ecommerce.Models;

namespace ecommerce.Cart
{
    public interface IProductService
    {
        Product GetProductById(int productId);

    }
}
