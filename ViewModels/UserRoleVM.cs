using System.ComponentModel.DataAnnotations;

namespace DeckMaster.ViewModels
{
    public class UserRoleVM
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string? RoleName { get; set; }
    }
}