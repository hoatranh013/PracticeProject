
using APIWeapon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWeapon.Configuration
{
    public class CharacterConfiguration : IEntityTypeConfiguration<CharacterModel>
    {
        public void Configure(EntityTypeBuilder<CharacterModel> builder)
        {
            builder.HasData(
                new CharacterModel
                {
                    Id = 4214124,
                    CharacterName = "Terenas Menathil",
                    Password = "bb0f7e021d52a4e31613d463fc0525d8",
                    Gmail = "chaunguyengiang2000@gmail.com",
                    Class = "Paladin",
                    Rule = "King"
                }
            );
        }
    }
}