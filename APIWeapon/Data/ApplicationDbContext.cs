
using Microsoft.EntityFrameworkCore;
using APIWeapon.Models;
using APIWeapon.Configuration;

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

        public DbSet<ResettingPasswordModel> ResettingPasswordModels { get; set; }

        public DbSet<ResettingToken> ResettingTokens { get; set; }
        public DbSet<PreviousPasswordModel> PreviousPasswordModels { get; set; }
        public DbSet<SuggestionGameModel> SuggestionGameModels { get; set; }
        public DbSet<SuggestionClassModel> SuggestionClassModels { get; set; }
        public DbSet<SuggestionWeaponModel> SuggestionWeaponModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CharacterConfiguration());
            modelBuilder.ApplyConfiguration(new WeaponConfiguration());


            modelBuilder.Entity<CharacterModel>(e =>
            {
                e.ToTable("dbo.CharacterModels");
                e.HasKey(dh => dh.Id);
                e.Property(dh => dh.CharacterName).IsRequired().HasMaxLength(100);
                e.HasIndex(dh => dh.CharacterName).IsUnique(true);
                e.Property(dh => dh.Password).IsRequired().HasMaxLength(50);
                e.Property(dh => dh.Gmail).IsRequired().HasMaxLength(50);
                e.Property(dh => dh.Class).IsRequired().HasMaxLength(50);
                e.Property(dh => dh.Rule).IsRequired().HasMaxLength(50);

            });

            modelBuilder.Entity<WeaponModel>(e =>
            {
                e.ToTable("dbo.WeaponModels");
                e.HasKey(dh => dh.WeaponId);
                e.Property(dh => dh.WeaponName).IsRequired().HasMaxLength(100);
                e.Property(dh => dh.WeaponAttack).IsRequired().HasMaxLength(50);
                e.Property(dh => dh.WeaponDefense).IsRequired().HasMaxLength(50);
                e.Property(dh => dh.WeaponAttribute).IsRequired().HasMaxLength(50);

            });
        }




    }
}
