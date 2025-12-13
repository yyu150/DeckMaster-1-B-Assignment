using DeckMaster.Data;
using DeckMaster.ViewModels;

namespace DeckMaster.Repositories
{
    public class ProductRepo
    {
        private readonly ApplicationDbContext _context;

        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get specific product in the database.
        public ProductVM? GetProduct(int id)
        {
            ProductVM? product = _context.Products.Select(p => new ProductVM
            {
                ID = p.ID,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                Currency = p.Currency,
                Image = p.Image
            }).FirstOrDefault(p => p.ID == id);

            return product;
        }

        // Get all products in the database.
        public IEnumerable<ProductVM> GetAllProducts()
        {
            IEnumerable<ProductVM> products = _context.Products.Select(p => new ProductVM
            {
                ID = p.ID,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                Currency = p.Currency,
                Image = p.Image
            });

            return products;
        }
    }
}

