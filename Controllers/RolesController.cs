using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeckMaster.Repositories;
using DeckMaster.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace DeckMaster.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class RolesController : Controller
    {
        private readonly RoleRepo _roleRepo;
        
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleRepo roleRepo, RoleManager<IdentityRole> roleManager)
        {
            _roleRepo = roleRepo;
            _roleManager = roleManager;
        }

        public IActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            var roles = _roleRepo.GetAllRolesVM();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new RoleVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                var ok = _roleRepo.CreateRole(roleVM.RoleName!);
                if (ok)
                {
                    return RedirectToAction(nameof(Index),
                        new { message = $"Successfully added {roleVM.RoleName}" });
                }

                ModelState.AddModelError("", "Role creation failed. It may already exist.");
            }

            return View(roleVM);
        }

        [HttpGet]
        public IActionResult Delete(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return RedirectToAction(nameof(Index), new { message = "Role name is required." });

            return View(new RoleVM { RoleName = roleName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(RoleVM vm)
        {
            if (vm == null || string.IsNullOrWhiteSpace(vm.RoleName))
                return RedirectToAction(nameof(Index), new { message = "Role name is required." });

            var role = await _roleManager.FindByNameAsync(vm.RoleName);
            if (role == null)
                return RedirectToAction(nameof(Index), new { message = "Role not found." });

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index), new { message = $"Delete failed: {errors}" });
            }

            return RedirectToAction(nameof(Index), new { message = "Role deleted successfully." });
        }
    }
}