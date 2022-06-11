using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class PreviousPasswordModel
    {
        [Key]
        public int DeId { get; set; }
        public string PreviousToken { get; set; } = String.Empty;
        public bool Handle { get; set; }
    }
}
