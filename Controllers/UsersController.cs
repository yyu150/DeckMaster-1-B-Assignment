using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeckMaster.Repositories;
using DeckMaster.ViewModels;

namespace DeckMaster.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class UsersController : Controller
    {
        private readonly UserRepo _userRepo;
        private readonly UserRoleRepo _userRoleRepo;
        private readonly RoleRepo _roleRepo;

        public UsersController(UserRepo userRepo, UserRoleRepo userRoleRepo, RoleRepo roleRepo)
        {
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _roleRepo = roleRepo;
        }

        // List all users
        public IActionResult Index()
        {
            var users = _userRepo.GetAllUsers();
            return View(users);
        }

        // Show roles for a user
        public async Task<IActionResult> Detail(string userName, string message = "")
        {
            ViewBag.UserName = userName;
            ViewBag.Message = message;
            var roles = await _userRoleRepo.GetUserRolesAsync(userName);
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create(string? email)
        {
            ViewBag.UserSelectList = _userRepo.GetUserSelectList(email);
            ViewBag.RoleSelectList = _roleRepo.GetRoleSelectList();
            return View(new UserRoleVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            if (ModelState.IsValid)
            {
                var ok = await _userRoleRepo.AddUserRoleAsync(userRoleVM.Email!, userRoleVM.RoleName!);
                if (ok)
                {
                    var message = $"{userRoleVM.RoleName} successfully added to {userRoleVM.Email}.";
                    return RedirectToAction(nameof(Detail), new { userName = userRoleVM.Email, message });
                }

                ModelState.AddModelError("", "Failed to add role to user (maybe already assigned).");
            }

            ViewBag.UserSelectList = _userRepo.GetUserSelectList(userRoleVM.Email);
            ViewBag.RoleSelectList = _roleRepo.GetRoleSelectList();
            return View(userRoleVM);
        }

        [HttpGet]
        public IActionResult Delete(string email, string roleName)
        {
            return View(new UserRoleVM { Email = email, RoleName = roleName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(UserRoleVM userRoleVM)
        {
            var ok = await _userRoleRepo.RemoveUserRoleAsync(userRoleVM.Email!, userRoleVM.RoleName!);
            var message = ok
                ? $"{userRoleVM.RoleName} successfully removed from {userRoleVM.Email}."
                : "Failed to remove role from user.";

            return RedirectToAction(nameof(Detail), new { userName = userRoleVM.Email, message });
        }
        
        
    }
}