using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class AddFriendNoti
    {
        [Key]
        public int AddFriId { get; set; }

        public string TheSender { get; set; } = String.Empty;
        public string TheReceiver { get; set; } = String.Empty;

        public string DateTime { get; set; } = String.Empty;

        public bool HandleOrNot { get; set; }

    }
}
