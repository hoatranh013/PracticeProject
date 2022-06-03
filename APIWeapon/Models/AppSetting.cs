using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWeapon.Models
{
    public class AppSetting
    {
        [Key]
        public int SecretId { get; set; }
        public string SecretKey { get; set; }
    }
}