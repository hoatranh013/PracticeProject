using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class SearchingFriend
    {
        [Key]
        public int SearchingFriendId { get; set; }
        public string SearchName { get; set; } = String.Empty;

    }
}
