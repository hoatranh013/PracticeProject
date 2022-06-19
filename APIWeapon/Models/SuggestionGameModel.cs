using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class SuggestionGameModel
    {
        [Key]
        public int SuggestionGameId { get; set; }
        public string SuggestionGameDescription { get; set; } = String.Empty;
        public string SuggestionGamePros { get; set; } = String.Empty;
        public string SuggestionGameCons { get; set; } = String.Empty;
        public int SuggestionGameRate { get; set; }

    }
}
