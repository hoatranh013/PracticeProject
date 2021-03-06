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
    [ApiController]
    [Route("api/[controller]")]
    public class ChangeAndResetControllers : IChangeAndResetControllers
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService mailService;
        public ChangeAndResetControllers(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor httpContextAccessor, IMailService mailService)
        {
            _db = db;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            this.mailService = mailService;
        }
        [HttpPost("Forget Password")]
        public async Task<string> Send(string myaccount, string mygmail)
        {
            var findout = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == myaccount);
            if (findout == null)
            {
                return "Character Is Not Valid";
            }
            else
            {
                var checkgmail = findout.Gmail;
                if (checkgmail != mygmail)
                {
                    return "Gmail Is Not Suitable";
                }
                else
                {
                    MailRequest requested = new MailRequest();
                    requested.ToEmail = findout.Gmail;
                    requested.Subject = "Code For Resetting Password";
                    requested.Body = GenerateTokenResetPassword(findout);
                    var tested = _db.PreviousPasswordModels.FirstOrDefault(s => s.Handle == false);
                    if (tested != null)
                    {
                        tested.Handle = true;
                        _db.SaveChanges();
                    }
                    if (tested == null)
                    {
                        _db.SaveChanges();
                    }
                    var newesttoken = new PreviousPasswordModel();
                    newesttoken.PreviousToken = requested.Body;
                    newesttoken.Handle = false;
                    _db.PreviousPasswordModels.Add(newesttoken);
                    _db.SaveChanges();
                    await mailService.SendEmailAsync(requested);
                    return "Check Your Gmail";
                }
            }
            return null;

        }
        [HttpPost("ForgetPasswordSuccessful/{id}")]
        public async Task<string> ResetPasswordSuccessful(string id)
        {
            var checkerrortoken = _db.PreviousPasswordModels.FirstOrDefault(s => s.PreviousToken == id && s.Handle == false);
            if (checkerrortoken != null)
            {
                var checkitnow = _db.ResettingTokens.FirstOrDefault(s => s.ResetToken == id);
                if (checkitnow == null)
                {
                    Random rnd = new Random();
                    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    var claims = jwtSecurityTokenHandler.ReadJwtToken(id).Claims;
                    if (claims != null)
                    {
                        var reseted = new ResettingPasswordModel();
                        reseted.RsCharacterName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.FamilyName)?.Value;
                        reseted.RsClass = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Gender)?.Value;
                        reseted.RsRule = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName)?.Value;
                        reseted.RsGmail = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Nonce)?.Value;
                        var findiden = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == reseted.RsCharacterName && s.Gmail == reseted.RsGmail && s.Rule == reseted.RsRule);
                        int NewPassword = rnd.Next();
                        MailRequest requested = new MailRequest();
                        requested.ToEmail = findiden.Gmail;
                        requested.Subject = "Here Is Your New Password";
                        requested.Body = NewPassword.ToString();
                        await mailService.SendEmailAsync(requested);
                        findiden.Password = GetMD5(NewPassword.ToString());
                        var addingresettoken = new ResettingToken();
                        addingresettoken.ResetToken = id;
                        _db.ResettingTokens.Add(addingresettoken);
                        _db.SaveChanges();
                        return "Check Your Gmail";
                    }
                    else
                    {
                        return "Invalid Token Or Token Has Expired";
                    }
                }
                else
                {
                    return "This Token Has Been Used";
                }
            }
            else
            {
                return "This Token Has Been Outdated";
            }
        }


        [HttpGet("Change Password")]
        public async Task<string> ChangePassword(string token, string oldpassword, string newpassword, string confirm)
        {
            var characterpresent = _db.CharacterModels.FirstOrDefault(s => s.Token == token);
            if (characterpresent != null)
            {
                string OPS = GetMD5(characterpresent.Password);
                if (GetMD5(oldpassword) != OPS)
                {
                    return "Wrong Password";
                }
                else
                {
                    if (newpassword != confirm)
                    {
                        return "Type New Password Again";
                    }
                    else
                    {
                        characterpresent.Password = GetMD5(newpassword);
                        _db.SaveChanges();
                        return "Change Password Successful";
                    }
                }
            }
            else
            {
                return "Identification Invalid";
            }
        }

        private string GenerateTokenResetPassword(CharacterModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["AppSettings:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.FamilyName, model.CharacterName),
                    new Claim(JwtRegisteredClaimNames.Gender, model.Class),
                    new Claim(JwtRegisteredClaimNames.GivenName, model.Rule),
                    new Claim(JwtRegisteredClaimNames.Exp, "abcdeg"),
                    new Claim(JwtRegisteredClaimNames.Nonce, model.Gmail),
                }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }


        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }

}

