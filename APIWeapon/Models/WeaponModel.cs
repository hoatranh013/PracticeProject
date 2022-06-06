using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class WeaponModel
    {
        [Key]
        public int WeaponId { get; set; }
        public string WeaponName { get; set; } = String.Empty;
        public int WeaponDefense { get; set; }
        public int WeaponAttack { get; set; } 
        public string WeaponAttribute { get; set; } = String.Empty;
        public string WeaponOwner { get; set; } = String.Empty;

    }
}
