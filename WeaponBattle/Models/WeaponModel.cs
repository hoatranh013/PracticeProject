namespace WeaponBattle.Models
{
    public class WeaponModel
    {
        public int WeaponId { get; set; }

        public string WeaponName { get; set; } = String.Empty;

        public int WeaponAttack { get; set; }
        public int WeaponDefense { get; set; }
        public string WeaponAttribute { get; set; } = String.Empty;
        public string WeaponOwner { get; set; } = String.Empty;
    }
}
