using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class NotificationModel
    {
        [Key]
        public int NotificationId { get; set; }
        public string TheSender { get; set; } = String.Empty;
        public string TheReceiver { get; set; } = String.Empty;

        public string WeaponTrade { get; set; } = String.Empty;

        public string DateTime { get; set; } = String.Empty;

        public bool HandleOrNot { get; set; }


    }
}
