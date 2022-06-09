using System.ComponentModel.DataAnnotations;

namespace APIWeapon.Models
{
    public class WhatLoginModel
    {
        public string CharacterName { get; set; } = String.Empty;
        public string Class { get; set; } = String.Empty;
        public string Rule { get; set; } = String.Empty;
    }
}
