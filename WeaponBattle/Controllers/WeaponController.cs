
using WeaponBattle.Models;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WeaponBattle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponController : ControllerBase
    {
        private static List<WeaponModel> WeaponModels = new List<WeaponModel>
        {
        };

        [HttpGet]
        public ActionResult<List<WeaponModel>> Get()
        {
            return Ok(WeaponModels);
        }

        [HttpGet]
        [Route("{term}")]
        public ActionResult<WeaponModel> Get(string term)
        {
            var Weaponitem = WeaponModels.Find(item =>
                    item.WeaponName.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if (Weaponitem == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(WeaponModels);
            }
        }

        [HttpPost]
        public ActionResult Post(WeaponModel model)
        {
            var existingWeaponItem = WeaponModels.Find(model =>
                    model.WeaponName.Equals(model.WeaponName, StringComparison.InvariantCultureIgnoreCase));

            if (existingWeaponItem != null)
            {
                return Conflict("Cannot create the term because it already exists.");
            }
            else
            {
                WeaponModels.Add(model);
                var resourceUrl = Path.Combine(Request.Path.ToString(), Uri.EscapeUriString(model.WeaponName));
                return Created(resourceUrl, model);
            }
        }

        [HttpPut]
        public ActionResult Put(WeaponModel model)
        {
            var existingWeaponItem = WeaponModels.Find(model =>
            model.WeaponName.Equals(model.WeaponName, StringComparison.InvariantCultureIgnoreCase));

            if (existingWeaponItem == null)
            {
                return NotFound("Cannot update a nont existing term.");
            }
            else
            {
                existingWeaponItem.WeaponAttribute = model.WeaponAttribute;
                return Ok();
            }
        }

        [HttpDelete]
        [Route("{term}")]
        public ActionResult Delete(string term)
        {
            var WeaponFind = WeaponModels.Find(item =>
                   item.WeaponName.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if (WeaponFind == null)
            {
                return NotFound();
            }
            else
            {
                WeaponModels.Remove(WeaponFind);
                return NoContent();
            }
        }


    }
}
