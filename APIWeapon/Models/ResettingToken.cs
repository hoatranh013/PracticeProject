using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class ResettingToken
    {
        [Key]
        public int TkId { get; set; }
        public string ResetToken { get; set; } = String.Empty;
    }
}
