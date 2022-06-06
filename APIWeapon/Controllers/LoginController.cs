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
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        public LoginController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpGet("Kings/{id}")]
        public IActionResult KingsEndpoint(string id)
        {
            var checktoken = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (checktoken.Rule != "King")
            {
                return Ok("You Are Not The King");
            }
            else
            {
                return Ok("Welcome Back The King");
            }
        }

        [HttpGet("Knights/{id}")]
        public IActionResult KnightsEndpoint(string id)
        {
            var checktoken = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (checktoken.Rule != "Knight")
            {
                return Ok("You Are Not The Knight");
            }
            else
            {
                return Ok("Proud To Be A Knight - Unite We Stand");
            }
        }

        [HttpGet("Characters/{id}/Weapons")]

        public IActionResult CharacterWeaponList(string id)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var weaponsearcher = _db.WeaponModels.FirstOrDefault(s => s.WeaponOwner == findcharacter.CharacterName);
                if (weaponsearcher != null)
                {
                    string weaponowner = weaponsearcher.WeaponOwner;
                    IEnumerable<WeaponModel> weaponlist = _db.WeaponModels.Where(s => s.WeaponOwner == findcharacter.CharacterName);
                    return Ok(weaponlist);
                }
                else
                {
                    return Ok("You Don't Have Any Weapon");
                }
            }
            else
            {
                return Ok("You Don't Have The Permission");
            }
        }

        [HttpPost("Characters/{id}/Weapons/Create")]
        public IActionResult CreateWeapon(string id, WeaponModel model)
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
                    return Ok(model);
                }
                else
                {
                    return Ok("Weapon Already Exist");
                }
            }
            if (findcharacter.Rule == null)
            {
                return Ok("You Are Not The Knight In This Castle");
            }    
            else
            {
                return Ok("Only King Can Forge Weapon");
            }

        }

        [HttpPut("Characters/{id}/Weapon/Edit/{weaponid}")]
        public IActionResult EditWeapon(string id, WeaponModel model, int weaponid)
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
                    return Ok(weaponfind);
                }
                else
                {
                    return Ok("Cannot Find Weapon ID");
                }
            }
            
            if (findcharacter.Rule == null)
            {
                return Ok("You Are Not The Knight In This Castle");
            }
            else
            {
                return Ok("Only King Can Forge Weapon Again");
            }
        }

        [HttpDelete("Characters/{id}/Weapon/Delete/{weaponname}")]
        public IActionResult DeleteWeapon(string id, string weaponname)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter.Rule == "King")
            {
                var findweapon = _db.WeaponModels.FirstOrDefault(s => s.WeaponName == weaponname);
                if (findweapon != null)
                {
                    _db.WeaponModels.Remove(findweapon);
                    return Ok("Weapon Removed");
                }
                else
                {
                    return Ok("Weapon Does Not Exist");
                }

            }
            else
            {
                return Ok("You Are Not The King");
            }
        }


        [HttpGet("Characters/{id}/WeaponList")]
        public IActionResult WeaponPreview(string id)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                IEnumerable<WeaponModel> weaponlist = _db.WeaponModels.Where(s => s.WeaponOwner == "Terenas Menathil");
                return Ok(weaponlist);

            }
            else
            {
                return Ok("You Don't Have Permission To See The Weaponlist");
            }

        }

        [HttpPost("Characters/{id}/WeaponList/{weaponstring}")]
        public IActionResult WeaponBuying(string id, string weaponstring, NotificationModel tntfct)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var findking = _db.WeaponModels.FirstOrDefault(s => s.WeaponName == weaponstring);
                if (findking != null)
                {
                    tntfct.TheSender = findcharacter.CharacterName;
                    tntfct.TheReceiver = findking.WeaponOwner;
                    tntfct.DateTime = DateTime.Now.ToString();
                    tntfct.WeaponTrade = weaponstring;
                    tntfct.HandleOrNot = false;
                    _db.NotificationModels.Add(tntfct);
                    _db.SaveChanges();
                    return Ok("Wait For The King Response");
                }
                else
                {
                    return Ok("Weapon Does Not Exist");
                }
            }
            else
            {
                return Ok("Cannot Find The Dealers");
            }
        }

        [HttpGet("Characters/{id}/Notification")]
        public IActionResult ShowNotification(string id)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                IEnumerable<NotificationModel> notificationlist = _db.NotificationModels.Where(s => s.TheReceiver == findcharacter.CharacterName && s.HandleOrNot == false);
                return Ok(notificationlist);

            }
            else
            {
                return Ok("Cannot Find The Notification");
            }    
        }

        [HttpGet("Characters/{id}/Notification/{idrequest}")]
        public IActionResult HandleNotification(string id,int idrequest,string acceptornot)
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
                        return Ok("Trade Successful");
                        
                    }   
                    if((acceptornot == "no") && (_db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest).HandleOrNot == false))
                    {
                        _db.NotificationModels.FirstOrDefault(s => s.NotificationId == idrequest).HandleOrNot = true;
                        _db.SaveChanges();
                        return Ok("Trade Unsuccessful");
                    }    
                    else
                    {
                        return Ok("Cannot Find Out The Trading Behaviour");
                    }    
                }    
                else
                {
                    return Ok("Cannot Find Out The Trading Behaviour");
                }    
            }   
            else
            {
                return Ok("Cannot Handle The Notification");
            }    
        }

        [HttpGet("Character/{id}/Friend")]
        public IActionResult ShowFriend (string id)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                string naming = findcharacter.CharacterName;
                IEnumerable<FriendList> mylistfriend = _db.FriendLists.Where(s => s.TheOwnered == naming);
                if (mylistfriend != null)
                {
                    return Ok(mylistfriend);
                }
                else
                {
                    return Ok("You Don't Have Friend");
                }

            }
            else
            {
                return Ok("Cannot Find The Indentification");
            }

        }

        [HttpPost("Character/{id}/Friend/AddFriend/{pd}")]
            public IActionResult AddingFriend (string id,string pd,AddFriendNoti noti)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var findcontact = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == pd);
                if((findcontact != null) && (findcontact.CharacterName != findcharacter.CharacterName))
                {
                    noti.TheSender = findcharacter.CharacterName;
                    noti.TheReceiver = pd;
                    noti.DateTime = DateTime.Now.ToString();
                    noti.HandleOrNot = false;
                    _db.AddFriendNotis.Add(noti);
                    _db.SaveChanges();
                    return Ok("Waiting For The Respond");

                }
                if((findcontact.CharacterName == findcharacter.CharacterName) && ((findcontact != null)))
                {
                    return Ok("You Are Adding Friend With Yourself");
                }
                else
                {
                    return Ok("Cannot Find The Friend Request");
                }

            }
            else
            {
                return Ok("Cannot Find The Indentification");
            }

        }

        [HttpGet("Character/{id}/Friend/Notification")]
            public IActionResult FriendNotification(string id)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                IEnumerable<AddFriendNoti> findnotis = _db.AddFriendNotis.Where(s => s.TheReceiver == findcharacter.CharacterName && s.HandleOrNot == false);
                if (findnotis != null)
                {
                    return Ok(findnotis);
                }
                else
                {
                    return Ok("You Don't Have Any Notification");
                }

            }
            else
            {
                return Ok("Cannot Find The Identification");
            }

        }

        [HttpPost("Character/{id}/Friend/Notification/{it}")]
            public IActionResult AddFriendOrNot(string id, int it, string yesorno, FriendList makeone)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var findnotirequest = _db.AddFriendNotis.FirstOrDefault(s => s.AddFriId == it);
                if (yesorno == "Yes")
                {
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
                    return Ok("Add Friend Successfully");

                }
                if (yesorno == "No")
                {
                    findnotirequest.HandleOrNot = true;
                    _db.SaveChanges();
                    return Ok("Refusing Add Friend");

                }
                else
                {
                    return Ok("Wrong Communication");
                }

            }
            else
            {
                return Ok("Cannot Find The Identification");
            }
        }





        [HttpGet("Public")]
        public IActionResult Public()
        {
            return Ok("Welcome To Weapon Backsmith Here You Can Trade Weapon Each Other");
        }



        private CharacterModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new CharacterModel
                {
                    CharacterName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Class = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Rule = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}