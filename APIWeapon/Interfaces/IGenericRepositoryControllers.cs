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
using System.Linq.Expressions;

namespace APIWeapon.Interfaces
{
    public interface IGenericRepositoryControllers<T> where T : class
    {
        void Add(T entity,string id );

        void Update(T entity,string id);

        void Delete(T entity,string id);

        IEnumerable<T> All(string id);

    }
}