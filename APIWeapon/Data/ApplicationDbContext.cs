
using Microsoft.EntityFrameworkCore;
using APIWeapon.Models;

namespace APIWeapon.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<CharacterModel> CharacterModels { get; set; }
        public DbSet<LoginModel> LoginModels { get; set; }

        public DbSet<WeaponModel> WeaponModels { get; set; }

        public DbSet<NotificationModel> NotificationModels { get; set; }

        public DbSet<FriendList> FriendLists { get; set; }
        public DbSet<AddFriendNoti> AddFriendNotis { get; set; }

        public DbSet<SearchingWeapon> SearchingWeapons { get; set; }

        public DbSet<SearchingFriend> SearchingFriends { get; set; }




    }
}
