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
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace APIWeapon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericRepositoryControllers<T> : IGenericRepositoryControllers<T> where T : class
    {
        protected readonly ApplicationDbContext context;
        public GenericRepositoryControllers(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost("Characters/{id}")]
        public virtual void Add(T entity, string id)
        {
            var findcharacter = context.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                //Expression<Func<T, bool>> predicate
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Username Invalid");
            }
        }

        [HttpPut("Characters/{id}")]
        public virtual void Update(T entity,string id)
        {
            var findcharacter = context.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                context.Entry(entity).State = EntityState.Modified;
                context.Set<T>().Update(entity);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Username Invalid");
            }
        }
        public IEnumerable<T> All(string id)
        {
            var findcharacter = context.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                return context.Set<T>().ToList();
            }
            else
            {
                return null;
            }

        }
        [HttpDelete("Characters/{id}")]
        public void Delete(T entity, string id)
        {
            var findcharacter = context.CharacterModels.FirstOrDefault(s => s.Token == id);
            if (findcharacter != null)
            {
                context.Set<T>().Remove(entity);
            }
            else
            {
                Console.WriteLine("Username Invalid");
            }
        }
    }
}