using DeckMaster.Data;
using DeckMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeckMaster.Controllers
{
    [Authorize(Roles = "Manager")]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? searchEmail,
            string? sortField,
            string? sortDir,
            int page = 1)
        {
            const int pageSize = 8;

            IQueryable<Transaction> query = _context.Transactions.AsNoTracking();

            // Search by PayerEmail
            if (!string.IsNullOrWhiteSpace(searchEmail))
            {
                searchEmail = searchEmail.Trim().ToLower();
                query = query.Where(t => t.PayerEmail != null &&
                                         t.PayerEmail.ToLower().Contains(searchEmail));
            }

            // Sorting 
            sortField = string.IsNullOrWhiteSpace(sortField) ? "date" : sortField.ToLower();
            sortDir = string.IsNullOrWhiteSpace(sortDir) ? "desc" : sortDir.ToLower(); 

            bool asc = sortDir == "asc";

            query = sortField switch
            {
                "amount" => asc ? query.OrderBy(t => t.Amount) : query.OrderByDescending(t => t.Amount),
                "date" => asc ? query.OrderBy(t => t.CreatedAt) : query.OrderByDescending(t => t.CreatedAt),
                _ => query.OrderByDescending(t => t.CreatedAt)
            };
            
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (page < 1) page = 1;
            if (totalPages > 0 && page > totalPages) page = totalPages;

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            ViewBag.SearchEmail = searchEmail;
            ViewBag.SortField = sortField;
            ViewBag.SortDir = sortDir;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            return View(items);
        }
    }
}