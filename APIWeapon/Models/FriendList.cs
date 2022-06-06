using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class FriendList
    {
        [Key]
        public int FriendListId { get; set; }
        public string TheOwnered { get; set; } = String.Empty;
        public string FriendName { get; set; } = String.Empty;

    }
}
