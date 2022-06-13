using APIWeapon.Data;
using APIWeapon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIWeapon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ILoginController
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }



        [HttpGet("Kings/{id}")]
        public async Task<string> KingsEndpoint(string id)
        {
            var checktoken = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (checktoken.Rule != "King")
            {
                return ("You Are Not The King");
            }
            else
            {
                var model = GetCurrentUser(id);
                return "Name - " + model.CharacterName + "Class - " + model.Class + "Rule - " + model.Rule;

            }
        }

        [HttpGet("Knights/{id}")]
        public async Task<string> KnightsEndpoint(string id)
        {
            var checktoken = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (checktoken.Rule != "Knight")
            {
                return ("You Are Not The Knight");
            }
            else
            {
                var model = GetCurrentUser(id);
                return "Name - " + model.CharacterName + "Class - " + model.Class + "Rule - " + model.Rule;
            }
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
        public async Task<string> HandleNotification(string id,int idrequest,string acceptornot)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var findnoti = _db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest);
                if (findnoti != null)
                {
                    if((acceptornot == "yes") && (_db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest).HandleOrNot == false))
                    {
                        var trading = _db.WeaponModels.FirstOrDefault(s => s.WeaponName == findnoti.WeaponTrade);
                        _db.WeaponModels.FirstOrDefault(s => s.WeaponName == findnoti.WeaponTrade).WeaponOwner = findnoti.TheSender;
                        _db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest).HandleOrNot = true;
                        _db.SaveChanges();
                        return "Trade Successful";
                        
                    }   
                    if((acceptornot == "no") && (_db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest).HandleOrNot == false))
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

        [HttpGet("Character/{id}/Friend")]
        public async Task<IEnumerable<FriendList>> ShowFriend (string id)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                string naming = findcharacter.CharacterName;
                IEnumerable<FriendList> mylistfriend = _db.FriendLists.Where(s => s.TheOwnered == naming);
                if (mylistfriend != null)
                {
                    return mylistfriend;
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

        [HttpPost("Character/{id}/Friend/AddFriend/{pd}")]
            public async Task<string> AddingFriend (string id,string pd)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var findcontact = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == pd);
                if((findcontact != null) && (findcontact.CharacterName != findcharacter.CharacterName))
                {
                    var checkfriend = _db.FriendLists.FirstOrDefault(s => s.TheOwnered == findcharacter.CharacterName && s.FriendName == findcontact.CharacterName);
                    if (checkfriend == null)
                    {
                        var noti = new AddFriendNoti();
                        noti.TheSender = findcharacter.CharacterName;
                        noti.TheReceiver = pd;
                        noti.DateTime = DateTime.Now.ToString();
                        noti.HandleOrNot = false;
                        _db.AddFriendNotis.Add(noti);
                        _db.SaveChanges();
                        return "Waiting For The Respond";
                    }
                    else
                    {
                        return "You And This Person Already Friend With Each Other";
                    }

                }
                if((findcontact.CharacterName == findcharacter.CharacterName) && ((findcontact != null)))
                {
                    return "You Are Adding Friend With Yourself";
                }
                else
                {
                    return "Cannot Find The Friend Request";
                }

            }
            else
            {
                return "Cannot Find The Indentification";
            }

        }

        [HttpGet("Character/{id}/Friend/Notification")]
            public async Task<IEnumerable<AddFriendNoti>> FriendNotification(string id)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                IEnumerable<AddFriendNoti> findnotis = _db.AddFriendNotis.Where(s => s.TheReceiver == findcharacter.CharacterName && s.HandleOrNot == false);
                if (findnotis != null)
                {
                    return findnotis;
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

        [HttpPost("Character/{id}/Friend/Notification/{it}")]
            public async Task<string> AddFriendOrNot(string id, int it, string yesorno)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var findnotirequest = _db.AddFriendNotis.FirstOrDefault(s => s.AddFriId == it);
                if (yesorno == "Yes")
                {
                    var makeone = new FriendList();
                    var maketwo = new FriendList();
                    findnotirequest.HandleOrNot = true;
                    _db.SaveChanges();
                    makeone.TheOwnered = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == findnotirequest.TheSender).CharacterName;
                    makeone.FriendName = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == findnotirequest.TheReceiver).CharacterName;
                    _db.FriendLists.Add(makeone);
                    _db.SaveChanges();
                    maketwo.TheOwnered = makeone.FriendName;
                    maketwo.FriendName = makeone.TheOwnered;
                    _db.FriendLists.Add(maketwo);
                    _db.SaveChanges();
                    return "Add Friend Successfully";

                }
                if (yesorno == "No")
                {
                    findnotirequest.HandleOrNot = true;
                    _db.SaveChanges();
                    return "Refusing Add Friend";

                }
                else
                {
                    return "Wrong Communication";
                }

            }
            else
            {
                return "Cannot Find The Identification";
            }
        }

        [HttpDelete("Character/{id}/Friend/Delete/{bt}")]
        public async Task<string> DeleteFriend(string id,string bt)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var findfriend = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == bt);
                if (findfriend == null)
                {
                    return "Character Does Not Exist";
                }
                if (findfriend.CharacterName == bt)
                {
                    return "You Are Implementing In Yourself";
                }
                else
                {
                    var contacter = _db.FriendLists.FirstOrDefault(s => s.TheOwnered == findcharacter.CharacterName && s.FriendName == bt);
                    if (contacter == null)
                    {
                        return "You And This Character Are Not Friend With Each Other";
                    }
                    else
                    {
                        var contacted = _db.FriendLists.FirstOrDefault(s => s.TheOwnered == bt && s.FriendName == findcharacter.CharacterName);
                        _db.FriendLists.Remove(contacter);
                        _db.FriendLists.Remove(contacted);
                        _db.SaveChanges();
                        return "You And This Character Are No Longer Friend";
                    }
                }
            }
            else 
            {
                return "Cannot Find The Identification";
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

        [HttpPost("Character/{id}/FriendList/Searching/{pt}")]
        public async Task<IEnumerable<FriendList>> SearchFriend(string id, string pt)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                IEnumerable<FriendList> searchfriend = _db.FriendLists.Where(s => s.FriendName!.Contains(pt) && s.FriendName != findcharacter.CharacterName);
                if (searchfriend.Count() != 0)
                {
                    return searchfriend;
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









        private WhatLoginModel GetCurrentUser(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var claims = jwtSecurityTokenHandler.ReadJwtToken(token).Claims;

            return new WhatLoginModel
            {
                    CharacterName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value,
                    Class = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value,
                    Rule = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value,
            };
        }
    }
}