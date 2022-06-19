
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
    public class WeaponConfiguration : IEntityTypeConfiguration<WeaponModel>
    {
        public void Configure(EntityTypeBuilder<WeaponModel> builder)
        {
            builder.HasData(
                new WeaponModel
                {
                    WeaponId = 4134322,
                    WeaponName = "Stormbreaker - Thunder Hummer",
                    WeaponAttack = 1500,
                    WeaponDefense = 1800,
                    WeaponAttribute = "Light",
                    WeaponOwner = "Terenas Menathil"
                }
            );
        }
    }
}