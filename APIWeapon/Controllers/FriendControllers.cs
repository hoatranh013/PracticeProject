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
    public class FriendControllers :IFriendControllers
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FriendControllers(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("Character/{id}/Friend")]
        public async Task<IEnumerable<FriendList>> ShowFriend(string id)
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
        public async Task<string> AddingFriend(string id, string pd)
        {
            var findcharacter = _db.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                var findcontact = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == pd);
                if ((findcontact != null) && (findcontact.CharacterName != findcharacter.CharacterName))
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
                if ((findcontact.CharacterName == findcharacter.CharacterName) && ((findcontact != null)))
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
        public async Task<string> DeleteFriend(string id, string bt)
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
    }
}
