using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class ResettingPasswordModel
    {
        public string RsCharacterName { get; set; } = String.Empty;
        public string RsClass { get; set; } = String.Empty;
        public string RsRule { get; set; } = String.Empty;

        public string RsGmail { get; set; } = String.Empty;
    }
}
