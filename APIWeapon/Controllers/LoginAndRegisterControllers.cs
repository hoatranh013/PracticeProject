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
        public class LoginAndRegisterControllers : ILoginAndRegisterControllers
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService mailService;
        public LoginAndRegisterControllers(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor httpContextAccessor, IMailService mailService)
        {
            _db = db;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            this.mailService = mailService;
        }
        [HttpPost]
        public async Task<CharacterModel> Register(CharacterModel model)
        {
            var existingcharacter = _db.CharacterModels.FirstOrDefault(s =>
            s.CharacterName == model.CharacterName);

            if (existingcharacter != null)
            {
                return null;
            }
            else
            {
                model.Password = GetMD5(model.Password);
                _db.CharacterModels.Add(model);
                _db.SaveChanges();
                return model;
            }
        }

        [HttpPost("Login")]
        public async Task<ApiResponse> Validate([FromBody] LoginModel model)
        {
            var characterpresent = _db.CharacterModels.FirstOrDefault(s => s.CharacterName == model.UserName && s.Password == GetMD5(model.Password));
            if (characterpresent == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username/password"
                };
            }
            else
            {
                var token = GenerateToken(characterpresent);
                _db.CharacterModels.FirstOrDefault(s => s.CharacterName == model.UserName && s.Password == GetMD5(model.Password)).Token = token;
                _db.SaveChanges();
                var abcd = _httpContextAccessor.HttpContext?.User?.FindFirst(o => o.Type == ClaimTypes.Role);
                return
                new ApiResponse
                {
                    Success = true,
                    Data = token,
                    Message = GetClaim(token)
                };
            }
        }

        [HttpGet("Logout")]
        public async Task<string> Logout(string token)
        {
            var characterpresent = _db.CharacterModels.FirstOrDefault(s => s.Token == token);
            if (characterpresent != null)
            {
                characterpresent.Token = "";
                _db.SaveChanges();
                return "Logout Successfull";
            }
            else
            {
                return "Logout Failure";
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
