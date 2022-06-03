
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




    }
}
