using APIWeapon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APIWeapon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        [HttpGet("Kings")]
        [Authorize(Roles = "King")]
        public IActionResult KingsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Welcome Back King");
        }


        [HttpGet("Captains")]
        [Authorize(Roles = "Captain")]
        public IActionResult CaptainsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Welcome Back Captain");
        }

        [HttpGet("Knights")]
        [Authorize(Roles = "Knight")]
        public IActionResult KnightsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Welcome Back Knight");
        }

        [HttpGet("Public")]
        public IActionResult Public()
        {
            return Ok("Welcome To Weapon Castle");
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