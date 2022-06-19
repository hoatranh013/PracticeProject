using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class SuggestionWeaponModel
    {
        [Key]
        public int SuggestWeaponId { get; set; }
        public string SuggestWeaponName { get; set; } = String.Empty;
        public string SuggestWeaponEffect { get; set; } = String.Empty;
        public string SuggestWeaponAttribute { get; set; } = String.Empty;
        public string SuggestWeaponDescription { get; set; } = String.Empty;

    }
}
