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
    public class IdentificationControllers : IIdentificationControllers
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IdentificationControllers(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor httpContextAccessor)
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
