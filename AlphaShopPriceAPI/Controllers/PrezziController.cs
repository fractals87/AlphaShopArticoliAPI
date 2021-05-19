using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaShopPriceAPI.Dtos;
using AlphaShopPriceAPI.Models;
using AlphaShopPriceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlphaShopPriceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/prezzi")]
    [Authorize(Roles = "ADMIN, USER")]
    public class PrezziController : Controller
    {
        private readonly IPrezziRepository prezziRepository;

        public PrezziController(IPrezziRepository prezziRepository)
        {
            this.prezziRepository = prezziRepository;
        }

        [HttpGet("{codart}/{idlist?}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPriceCodArt(string CodArt, string IdList)
        {
            IdList = (IdList == null) ? "1" : IdList;

            if (!this.prezziRepository.PrezzoExists(CodArt, IdList))
            {
                return NotFound();
            }

            var prezzo = await this.prezziRepository.SelPrezzoByCodArtAndList(CodArt, IdList);

            var prezzoDto = new PrezziDTO
            {
                Id = prezzo.Id,
                Listino = prezzo.IdList,
                Prezzo = prezzo.Prezzo
            };

            return Ok(prezzoDto);
        }

        [HttpDelete("elimina/{codart}/{idlist}")]
        [ProducesResponseType(201, Type = typeof(InfoMsg))]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeletePrice(string idlist, string codart)
        {
            if (idlist == "" || codart == "")
            {
                return BadRequest(new InfoMsg(DateTime.Today, $"E' necessario inserire l'id del listino e/o il codice articolo da eliminare!"));
            }

            //verifichiamo che i dati siano stati regolarmente eliminati dal database
            if (!await prezziRepository.DelPrezzoListino(codart, idlist))
            {
                return StatusCode(500, new InfoMsg(DateTime.Today, $"Ci sono stati problemi nella eliminazione del prezzo del codice {codart}.  "));
            }

            return Ok(new InfoMsg(DateTime.Today, $"Eliminazione del prezzo Listino {idlist} del codice {codart} eseguita con successo!"));

        }
    }
}
