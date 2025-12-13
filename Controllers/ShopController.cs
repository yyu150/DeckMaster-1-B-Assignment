using DeckMaster.Data;
using DeckMaster.Repositories;
using DeckMaster.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PayPalDemo.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProductRepo _productRepo;

        // Constructor receives the context through dependency injection.
        public ShopController(ApplicationDbContext context, ProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductVM> products = _productRepo.GetAllProducts();

            return View("Index", products);
        }
    }
}
