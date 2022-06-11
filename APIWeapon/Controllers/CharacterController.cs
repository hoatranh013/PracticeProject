
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
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService mailService;
        public CharacterController(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor httpContextAccessor, IMailService mailService)
        {
            _db = db;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            this.mailService = mailService;
        }

        [HttpPost("Forget Password")]
        public async Task<IActionResult> Send(string myaccount, string mygmail)
        {
                var findout = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == myaccount);
                if (findout == null)
                {
                    return Ok("Character Is Not Valid");
                }
                else
                {
                    var checkgmail = findout.Gmail;
                    if (checkgmail != mygmail)
                    {
                        return Ok("Gmail Is Not Suitable");
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
                        return Ok("Check Your Gmail");
                    }
                }
                return Ok();

        }
        [HttpPost("ForgetPasswordSuccessful/{id}")]
        public async Task<IActionResult> ResetPasswordSuccessful (string id)
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
                        return Ok("Check Your Gmail");
                    }
                    else
                    {
                        return Ok("Invalid Token Or Token Has Expired");
                    }
                }
                else
                {
                    return Ok("This Token Has Been Used");
                }
            }
            else
            {
                return Ok("This Token Has Been Outdated");
            }
        }


        [HttpGet("Change Password")]
        public ActionResult ChangePassword(string token, string oldpassword, string newpassword, string confirm)
        {
            var characterpresent = _db.CharacterModels.FirstOrDefault(s => s.Token == token);
            if (characterpresent != null)
            {
                string OPS = GetMD5(characterpresent.Password);
                if (GetMD5(oldpassword) != OPS)
                {
                    return Ok("Wrong Password");
                }
                else
                {
                    if (newpassword != confirm)
                    {
                        return Ok("Type New Password Again");
                    }
                    else
                    {
                        characterpresent.Password = GetMD5(newpassword);
                        _db.SaveChanges();
                        return Ok("Change Password Successful");
                    }
                }
            }
            else
            {
                return Ok("Identification Invalid");
            }
        }


        [HttpPost]
        public ActionResult Register(CharacterModel model)
        {
            var existingcharacter = _db.CharacterModels.FirstOrDefault(s =>
            s.CharacterName == model.CharacterName);

            if (existingcharacter != null)
            {
                return Conflict("Cannot create the character because it already exists.");
            }
            else
            {
                model.Password = GetMD5(model.Password);
                _db.CharacterModels.Add(model);
                _db.SaveChanges();
                var resourceUrl = Path.Combine(Request.Path.ToString(), Uri.EscapeUriString(model.CharacterName));
                return Created(resourceUrl, model);
            }
        }

        [HttpPost("Login")]
        public ActionResult Validate([FromBody] LoginModel model)
        {
            var characterpresent = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == model.UserName && s.Password == GetMD5(model.Password));
            if (characterpresent == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username/password"
                });
            }
            else
            {
                var token = GenerateToken(characterpresent);
                _db.CharacterModels.FirstOrDefault(s => s.CharacterName == model.UserName && s.Password == GetMD5(model.Password)).Token = token;
                _db.SaveChanges();
                var abcd = _httpContextAccessor.HttpContext?.User?.FindFirst(o => o.Type == ClaimTypes.Role);
                return Ok(
                new ApiResponse
                {
                    Success = true,
                    Data = token,
                    Message = GetClaim(token)
                }) ;
            }
        }

        [HttpGet("Logout")]
        public ActionResult Logout(string token)
        {
            var characterpresent = _db.CharacterModels.FirstOrDefault(s => s.Token == token);
            if (characterpresent != null)
            {
                characterpresent.Token = "";
                _db.SaveChanges();
                return Ok("Logout Successfull");
            }
            else
            {
                return Ok("Logout Failure");
            }
        }


        private string GenerateToken(CharacterModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["AppSettings:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, model.CharacterName),
                    new Claim(JwtRegisteredClaimNames.Sub, model.Class),
                    new Claim(JwtRegisteredClaimNames.Jti, model.Rule)
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
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

        public static string GetClaim(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var claims = jwtSecurityTokenHandler.ReadJwtToken(token).Claims;
                
            return claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        }
    }
}
