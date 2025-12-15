using Microsoft.AspNetCore.Mvc.Rendering;
using DeckMaster.Data;
using DeckMaster.ViewModels;

namespace DeckMaster.Repositories
{
    public class UserRepo
    {
        private readonly ApplicationDbContext _db;

        public UserRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<UserVM> GetAllUsers()
        {
            return _db.Users
                .Select(u => new UserVM { Email = u.Email })
                .ToList();
        }

        public SelectList GetUserSelectList(string? email)
        {
            var users = GetAllUsers().Select(u => new SelectListItem
            {
                Value = u.Email,
                Text = u.Email
            });

            return new SelectList(users, "Value", "Text", email);
        }
    }
}