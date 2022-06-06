
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

namespace APIWeapon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        public CharacterController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
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
                return Ok(
                new ApiResponse
                {
                    Success = true,
                    Data = token,
                    Message = "Success"
                });
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
            if (jwtTokenHandler == null)
            {

            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:SecretKey"]));
            if (securityKey == null)
            {

            }
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            if (credentials == null)
            {

            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.CharacterName),
                new Claim(ClaimTypes.Email, model.Password),
                new Claim(ClaimTypes.Surname, model.Class),
                new Claim(ClaimTypes.Role, model.Rule)
            };

            var token = new JwtSecurityToken(_config["AppSettings:Issuer"],
              _config["AppSettings:Audience"], 
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);
            return jwtTokenHandler.WriteToken(token);
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
