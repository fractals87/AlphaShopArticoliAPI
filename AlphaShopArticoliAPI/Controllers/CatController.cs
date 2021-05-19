using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaShopArticoliAPI.DTO;
using AlphaShopArticoliAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlphaShopArticoliAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/cat")]
    public class CatController : ControllerBase
    {
        private readonly IArticoliRepository articolirepository;

        public CatController(IArticoliRepository articolirepository)
        {
            this.articolirepository = articolirepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoriaDto>))]
        public async Task<IActionResult> GetCat()
        {
            var catDto = new List<CategoriaDto>();

            var iva = await this.articolirepository.SelCat();

            foreach (var Iva in iva)
            {
                catDto.Add(new CategoriaDto
                {
                    Id = Iva.Id,
                    Descrizione = Iva.Descrizione
                });
            }

            return Ok(catDto);

        }
    }
}