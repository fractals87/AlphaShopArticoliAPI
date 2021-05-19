using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaShopPriceAPI.Models;
using AlphaShopPriceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlphaShopPriceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/listini")]
    [Authorize(Roles = "ADMIN, USER")]
    public class ListiniController : Controller
    {
        private readonly IListiniRepository listiniRepository;

        public ListiniController(IListiniRepository listiniRepository)
        {
            this.listiniRepository = listiniRepository;
        }

        [HttpGet("cerca/id/{idlist}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetListById(string idlist)
        {
            var listini = await this.listiniRepository.SelById(idlist);

            if (listini == null)
            {
                return NotFound(string.Format("Non è stato trovato il listino con il codice '{0}'", idlist));
            }

            return Ok(listini);
        }

        [HttpPost("inserisci")]
        [ProducesResponseType(201, Type = typeof(Listini))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]
        public IActionResult SaveListino([FromBody] Listini listino)
        {
            if (listino == null)
            {
                return BadRequest(new InfoMsg(DateTime.Today, "E' necessario inserire i dati dell'utente"));
            }

            //Verifichiamo che i dati siano corretti
            if (!ModelState.IsValid)
            {
                string ErrVal = "";

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        ErrVal += modelError.ErrorMessage + " - ";
                    }
                }

                return BadRequest(new InfoMsg(DateTime.Today, ErrVal));
            }

            //Contolliamo se l'articolo è presente
            var isPresent = listiniRepository.CheckListino(listino.Id);

            if (isPresent != null)
            {
                return StatusCode(422, new InfoMsg(DateTime.Today, $"Listino {listino.Id} presente in anagrafica! Impossibile utilizzare il metodo POST!"));
            }

            //verifichiamo che i dati siano stati regolarmente inseriti nel database
            if (!listiniRepository.InsListini(listino))
            {
                return StatusCode(500, new InfoMsg(DateTime.Today, $"Ci sono stati problemi nell'inserimento del listino {listino.Id}."));
            }

            return Ok(new InfoMsg(DateTime.Today, $"Inserimento listino {listino.Id} eseguito con successo!"));

        }

        [HttpPut("modifica")]
        [ProducesResponseType(201, Type = typeof(Listini))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]
        public IActionResult UpdateListino([FromBody] Listini listino)
        {
            if (listino == null)
            {
                return BadRequest(new InfoMsg(DateTime.Today, "E' necessario inserire i dati del listino"));
            }

            //Verifichiamo che i dati siano corretti
            if (!ModelState.IsValid)
            {
                string ErrVal = "";

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        ErrVal += modelError.ErrorMessage + " - ";
                    }
                }

                return BadRequest(new InfoMsg(DateTime.Today, ErrVal));
            }

            //Contolliamo se l'articolo è presente
            var isPresent = listiniRepository.CheckListino(listino.Id);

            if (isPresent == null)
            {
                return StatusCode(422, new InfoMsg(DateTime.Today, $"Listino {listino.Id} NON presente in anagrafica! Impossibile utilizzare il metodo PUT!"));
            }

            //verifichiamo che i dati siano stati regolarmente inseriti nel database
            if (!listiniRepository.UpdListini(listino))
            {
                return StatusCode(500, new InfoMsg(DateTime.Today, $"Ci sono stati problemi nell'inserimento del listino {listino.Id}."));
            }

            return Ok(new InfoMsg(DateTime.Today, $"Modifica listino {listino.Id} eseguita con successo!"));

        }

        [HttpDelete("elimina/{idlist}")]
        [ProducesResponseType(201, Type = typeof(InfoMsg))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        [ProducesResponseType(422, Type = typeof(InfoMsg))]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]
        public IActionResult DeleteListino(string idlist)
        {
            if (idlist == "")
            {
                return BadRequest(new InfoMsg(DateTime.Today, $"E' necessario inserire l'id del listino da eliminare!"));
            }

            //Contolliamo se l'articolo è presente (Usare il metodo senza Traking)
            var listino = listiniRepository.SelByIdNoTrack(idlist);

            if (listino == null)
            {
                return StatusCode(422, new InfoMsg(DateTime.Today, $"Listino {idlist} NON presente in anagrafica! Impossibile Eliminare!"));
            }

            //verifichiamo che i dati siano stati regolarmente eliminati dal database
            if (!listiniRepository.DelListini(listino))
            {
                //ModelState.AddModelError("", $"Ci sono stati problemi nella eliminazione dell'Articolo {articolo.CodArt}.  ");
                return StatusCode(500, new InfoMsg(DateTime.Today, $"Ci sono stati problemi nella eliminazione del listino {listino.Id}.  "));
            }

            return Ok(new InfoMsg(DateTime.Today, $"Eliminazione Listino {idlist} eseguita con successo!"));

        }
    }
}
