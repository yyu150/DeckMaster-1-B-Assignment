using DeckMaster.Data;
using DeckMaster.Models;
using DeckMaster.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DeckMaster.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRepo _productRepo;

        public ShopController(ApplicationDbContext context, ProductRepo productRepo)
        {
            _context = context;
            _productRepo = productRepo;
        }

        // Shop page
        public IActionResult Index()
        {
            var products = _productRepo.GetAllProducts();
            return View(products);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
                return BadRequest();

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(transaction);
        }

        // Payment confirmation 
        public IActionResult Confirmation(int id)
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
                return RedirectToAction(nameof(Index));

            return View(transaction);
        }
    }
}