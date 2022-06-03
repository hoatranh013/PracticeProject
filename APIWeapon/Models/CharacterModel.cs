using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class CharacterModel
    {
        [Key]
        public int Id { get; set; }
        public string CharacterName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Class { get; set; } = String.Empty;
        public string Rule { get; set; } = String.Empty;
    }
}
