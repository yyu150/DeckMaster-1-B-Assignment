using Microsoft.AspNetCore.Identity;
using DeckMaster.Data;
using DeckMaster.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeckMaster.Repositories
{
    public class RoleRepo
    {
        private readonly ApplicationDbContext _context;

        public RoleRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public SelectList GetRoleSelectList()
        {
            var roles = _context.Roles
                .Select(r => r.Name)
                .ToList();

            return new SelectList(roles);
        }

        public IEnumerable<RoleVM> GetAllRolesVM()
        {
            return _context.Roles
                .Select(r => new RoleVM { RoleName = r.Name })
                .ToList();
        }

        public IdentityRole? GetRole(string roleName)
        {
            return _context.Roles.FirstOrDefault(r => r.Name == roleName);
        }

        public bool DoesRoleHaveUsers(string roleName)
        {
            var role = GetRole(roleName);
            if (role == null) return false;

            return _context.UserRoles.Any(ur => ur.RoleId == role.Id);
        }

        public bool CreateRole(string roleName)
        {
            if (GetRole(roleName) != null) return false;

            _context.Roles.Add(new IdentityRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });

            _context.SaveChanges();
            return true;
        }

        public bool DeleteRole(string roleName)
        {
            var role = GetRole(roleName);
            if (role == null) return false;

            if (DoesRoleHaveUsers(roleName)) return false;

            _context.Roles.Remove(role);
            _context.SaveChanges();
            return true;
        }
    }
}