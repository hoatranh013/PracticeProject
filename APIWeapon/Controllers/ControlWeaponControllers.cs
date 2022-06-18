using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using APIWeapon.Data;
using APIWeapon.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Hl7.Fhir.Utility;
using APIWeapon.Models;
using APIWeapon.Services;
using APIWeapon.Interfaces;
namespace APIWeapon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlWeaponControllers : IControlWeaponControllers
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ControlWeaponControllers(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpPost("Characters/{id}/Weapons/Create")]
        public async Task<string> CreateWeapon(string id, WeaponModel model)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter.Rule == "King")
            {
                var createweapon = _db.WeaponModels.FirstOrDefault(s => s.WeaponName == model.WeaponName);
                if (createweapon == null)
                {
                    model.WeaponOwner = findcharacter.CharacterName;
                    _db.WeaponModels.Add(model);
                    _db.SaveChanges();
                    return "Name - " + model.WeaponName + "Attack - " + model.WeaponAttack + "Defense - " + model.WeaponDefense + "Attribute - " + model.WeaponAttribute;
                }
                else
                {
                    return "Weapon Already Exist";
                }
            }
            if (findcharacter.Rule == null)
            {
                return "You Are Not The Knight In This Castle";
            }
            else
            {
                return "Only King Can Forge Weapon";
            }

        }

        [HttpPut("Characters/{id}/Weapon/Edit/{weaponid}")]
        public async Task<string> EditWeapon(string id, WeaponModel model, int weaponid)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter.Rule == "King")
            {
                var weaponfind = _db.WeaponModels.FirstOrDefault(s => s.WeaponId == weaponid);
                if (weaponfind != null)
                {
                    weaponfind.WeaponDefense = model.WeaponDefense;
                    weaponfind.WeaponAttack = model.WeaponAttack;
                    weaponfind.WeaponAttribute = model.WeaponAttribute;
                    _db.SaveChanges();
                    return "Name - " + weaponfind.WeaponName + "Attack - " + weaponfind.WeaponAttack + "Defense - " + weaponfind.WeaponDefense + "Attribute - " + weaponfind.WeaponAttribute;
                }
                else
                {
                    return "Cannot Find Weapon ID";
                }
            }

            if (findcharacter.Rule == null)
            {
                return "You Are Not The Knight In This Castle";
            }
            else
            {
                return "Only King Can Forge Weapon Again";
            }
        }

        [HttpDelete("Characters/{id}/Weapon/Delete/{weaponname}")]
        public async Task<string> DeleteWeapon(string id, string weaponname)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter.Rule == "King")
            {
                var findweapon = _db.WeaponModels.FirstOrDefault(s => s.WeaponName == weaponname);
                if (findweapon != null)
                {
                    _db.WeaponModels.Remove(findweapon);
                    return "Weapon Removed";
                }
                else
                {
                    return "Weapon Does Not Exist";
                }

            }
            else
            {
                return "You Are Not The King";
            }
        }
    }
}
