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
using APIWeapon.Repository;
namespace APIWeapon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplimentControllers : ControllerBase
    {
        private readonly IUnitOfWork unitofwork;

        public ComplimentControllers(IUnitOfWork unitofwork)
        {
            this.unitofwork = unitofwork;
        }
        [HttpGet]
        public IEnumerable<SuggestionClassModel> GetAll()
        {
            return unitofwork.SuggestionClassModels.GetAll();
        }

    }
}
