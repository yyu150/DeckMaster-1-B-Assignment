using System.ComponentModel.DataAnnotations;

namespace DeckMaster.ViewModels
{
    public class UserVM
    {
        [Required]
        public string? Email { get; set; }
    }
}