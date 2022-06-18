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
    public class NotificationControllers : INotificationControllers
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NotificationControllers(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("Characters/{id}/Notification")]
        public async Task<string> ShowNotification(string id)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                IEnumerable<NotificationModel> notificationlist = _db.NotificationModels.Where(s => s.TheReceiver == findcharacter.CharacterName && s.HandleOrNot == false);
                return "The Receiver - " + notificationlist.FirstOrDefault().TheReceiver + "The Sender - " + notificationlist.FirstOrDefault().TheSender + "Weapon Trade - " + notificationlist.FirstOrDefault().WeaponTrade;

            }
            else
            {
                return "Cannot Find The Notification";
            }
        }

        [HttpGet("Characters/{id}/Notification/{idrequest}")]
        public async Task<string> HandleNotification(string id, int idrequest, string acceptornot)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var findnoti = _db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest);
                if (findnoti != null)
                {
                    if ((acceptornot == "yes") && (_db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest).HandleOrNot == false))
                    {
                        var trading = _db.WeaponModels.FirstOrDefault(s => s.WeaponName == findnoti.WeaponTrade);
                        _db.WeaponModels.FirstOrDefault(s => s.WeaponName == findnoti.WeaponTrade).WeaponOwner = findnoti.TheSender;
                        _db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest).HandleOrNot = true;
                        _db.SaveChanges();
                        return "Trade Successful";

                    }
                    if ((acceptornot == "no") && (_db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest).HandleOrNot == false))
                    {
                        _db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest).HandleOrNot = true;
                        _db.SaveChanges();
                        return "Trade Unsuccessful";
                    }
                    else
                    {
                        return "Cannot Find Out The Trading Behaviour";
                    }
                }
                else
                {
                    return "Cannot Find Out The Trading Behaviour";
                }
            }
            else
            {
                return "Cannot Handle The Notification";
            }
        }
    }
}
