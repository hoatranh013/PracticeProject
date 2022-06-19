using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class SuggestionClassModel
    {
        [Key]
        public int SuggestionClassId { get; set; }
        public string SuggestionClassName { get; set; } = String.Empty;
        public string SuggestionClassDescription { get; set; } = String.Empty;
        public string SuggestionClassAttribute { get; set; } = String.Empty;
        public string SuggestionClassWeapon { get; set; } = String.Empty;
        public string SuggestionClassEffect { get; set; } = String.Empty;

    }
}
