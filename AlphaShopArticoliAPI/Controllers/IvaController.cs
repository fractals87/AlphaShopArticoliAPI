using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaShopArticoliAPI.DTO;
using AlphaShopArticoliAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlphaShopArticoliAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/iva")]
    public class IvaController : Controller
    {
        private readonly IArticoliRepository articolirepository;

        public IvaController(IArticoliRepository articolirepository)
        {
            this.articolirepository = articolirepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IvaDto>))]
        public async Task<IActionResult> GetIva()
        {
            var ivaDto = new List<IvaDto>();

            var iva = await this.articolirepository.SelIva();

            foreach (var Iva in iva)
            {
                ivaDto.Add(new IvaDto
                {
                    IdIva = Iva.IdIva,
                    Descrizione = Iva.Descrizione
                });
            }

            return Ok(ivaDto);

        }


    }
}