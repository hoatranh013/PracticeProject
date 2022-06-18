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
    public class ShowWeaponControllers : IShowWeaponControllers
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ShowWeaponControllers(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Characters/{id}/Weapons")]
        public async Task<IEnumerable<WeaponModel>> CharacterWeaponList(string id)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var weaponsearcher = _db.WeaponModels.FirstOrDefault(s => s.WeaponOwner == findcharacter.CharacterName);
                if (weaponsearcher != null)
                {
                    string weaponowner = weaponsearcher.WeaponOwner;
                    IEnumerable<WeaponModel> weaponlist = _db.WeaponModels.Where(s => s.WeaponOwner == findcharacter.CharacterName);
                    return weaponlist;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [HttpPost("Character/{id}/SearchingWeapon")]
        public async Task<IEnumerable<WeaponModel>> SearchingWeapon(string id, string weap, string sch, int ind)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                if (weap == "WeaponName")
                {
                    IEnumerable<WeaponModel> FindWeapon = _db.WeaponModels.Where(s => s.WeaponName!.Contains(sch) && s.WeaponOwner == "Terenas Menathil");
                    return FindWeapon;
                }
                if (weap == "WeaponAttribute")
                {
                    IEnumerable<WeaponModel> FindWeapon = _db.WeaponModels.Where(s => s.WeaponAttribute == sch && s.WeaponOwner == "Terenas Menathil");
                    return FindWeapon;
                }
                if (weap == "WeaponAttack")
                {
                    IEnumerable<WeaponModel> FindWeapon = _db.WeaponModels.Where(s => s.WeaponAttack >= ind && s.WeaponOwner == "Terenas Menathil");
                    return FindWeapon;
                }
                if (weap == "WeaponDefense")
                {
                    IEnumerable<WeaponModel> FindWeapon = _db.WeaponModels.Where(s => s.WeaponDefense >= ind && s.WeaponOwner == "Terenas Menathil");
                    return FindWeapon;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


    }


}
