using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class SearchingWeapon
    {
        [Key]
        public int SearchingWeaponId { get; set; }
        public string SearchingWeaponName { get; set; } = String.Empty;
        public int SearchingWeaponDefense { get; set; }
        public int SearchingWeaponAttack { get; set; } 
        public string SearchingWeaponAttribute { get; set; } = String.Empty;

    }
}
