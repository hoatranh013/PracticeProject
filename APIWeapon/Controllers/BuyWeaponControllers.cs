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
    public class BuyWeaponControllers : IBuyWeaponControllers
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BuyWeaponControllers(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Characters/{id}/WeaponList")]
        public async Task<string> WeaponPreview(string id)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                IEnumerable<WeaponModel> weaponlist = _db.WeaponModels.Where(s => s.WeaponOwner == "Terenas Menathil");
                return "Name - " + weaponlist.FirstOrDefault().WeaponName + "Attack - " + weaponlist.FirstOrDefault().WeaponAttack + "Defense - " + weaponlist.FirstOrDefault().WeaponDefense + "Attribute - " + weaponlist.FirstOrDefault().WeaponAttribute;

            }
            else
            {
                return "You Don't Have Permission To See The Weaponlist";
            }

        }

        [HttpPost("Characters/{id}/WeaponList/{weaponstring}")]
        public async Task<string> WeaponBuying(string id, string weaponstring)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var findking = _db.WeaponModels.FirstOrDefault(s => s.WeaponName == weaponstring);
                if (findking != null)
                {
                    var tntfct = new NotificationModel();
                    tntfct.TheSender = findcharacter.CharacterName;
                    tntfct.TheReceiver = findking.WeaponOwner;
                    tntfct.DateTime = DateTime.Now.ToString();
                    tntfct.WeaponTrade = weaponstring;
                    tntfct.HandleOrNot = false;
                    _db.NotificationModels.Add(tntfct);
                    _db.SaveChanges();
                    return "Wait For The King Response";
                }
                else
                {
                    return "Weapon Does Not Exist";
                }
            }
            else
            {
                return "Cannot Find The Dealers";
            }
        }
    }
}
