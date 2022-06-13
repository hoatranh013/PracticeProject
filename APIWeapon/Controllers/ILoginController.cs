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

namespace APIWeapon.Controllers
{
    public interface ILoginController
    {
        Task<string> KingsEndpoint(string id);

        Task<string> KnightsEndpoint(string id);

        Task<IEnumerable<WeaponModel>> CharacterWeaponList(string id);
        Task<string> CreateWeapon(string id, WeaponModel model);
        Task<string> EditWeapon(string id, WeaponModel model, int weaponid);
        Task<string> WeaponBuying(string id, string weaponstring);
        Task<string> ShowNotification(string id);
        Task<string> HandleNotification(string id, int idrequest, string acceptornot);
        Task<IEnumerable<FriendList>> ShowFriend(string id);
        Task<string> AddingFriend(string id, string pd);

        Task<IEnumerable<AddFriendNoti>> FriendNotification(string id);
        Task<string> AddFriendOrNot(string id, int it, string yesorno);

        Task<string> DeleteFriend(string id, string bt);

        Task<IEnumerable<WeaponModel>> SearchingWeapon(string id, string weap, string sch, int ind);

        Task<IEnumerable<FriendList>> SearchFriend(string id, string pt);

    }
}