﻿using Microsoft.AspNetCore.Http;
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

namespace APIWeapon.Interfaces
{
    public interface INotificationControllers
    {
        Task<string> ShowNotification(string id);
        Task<string> HandleNotification(string id, int idrequest, string acceptornot);
    }
}