using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AlphaShopArticoliAPI.DTO;
using AlphaShopArticoliAPI.Dtos;
using AlphaShopArticoliAPI.Models;
using AlphaShopArticoliAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AlphaShopArticoliAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles ="ADMIN,USER")]
    public class ArticoliController : ControllerBase
    {
        private IArticoliRepository articoliRepository;
        private IMapper mapper;

        public ArticoliController(IArticoliRepository articoliRepository, IMapper mapper)
        {
            this.articoliRepository = articoliRepository;
            this.mapper = mapper;
        }

        [HttpGet("test")]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        public IActionResult TextConnex()
        {
            return Ok(new InfoMsg(DateTime.Today, "Test Connessione OK"));
        }

        [HttpGet]
        [HttpGet("getAllArticoli/")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Articoli>))]
        public async Task<IActionResult> getAllArticoli()
        {
            var articoli = await this.articoliRepository.getArticoli();
            return Ok(articoli);
        }

        [HttpGet("cerca/descrizione/{filter}/{IdList?}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ArticoliDto>))]
        public async Task<ActionResult<IEnumerable<ArticoliDto>>> GetArticoliByDesc(string filter,
            [FromQuery] string idCat, string IdList)
        {
            string accessToken = Request.Headers["Authorization"];

            IdList ??= "1";

            var articoliDto = new List<ArticoliDto>();

            var articoli = await this.articoliRepository.SelArticoliByDescrizione(filter, idCat);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (articoli.Count == 0)
            {
                return NotFound(string.Format("Non è stato trovato alcun articolo con il filtro '{0}'", filter));
            }

            
            foreach(var articolo in articoli)
            {
                PrezziDTO prezzoDTO = await getPriceArtAsync(articolo.CodArt, IdList, accessToken);

                articoliDto.Add(new ArticoliDto
                {
                    CodArt = articolo.CodArt,
                    Descrizione = articolo.Descrizione,
                    Um = articolo.Um,
                    CodStat = articolo.CodStat,
                    PzCart = articolo.PzCart,
                    PesoNetto = articolo.PesoNetto,
                    DataCreazione = articolo.DataCreazione,
                    IdStatoArt = articolo.IdStatoArt,
                    Categoria = (articolo.famAssort != null) ? articolo.famAssort.Descrizione : null,
                    Prezzo = (prezzoDTO == null) ? 0 : prezzoDTO.Prezzo
                });
            }
            return Ok(articoliDto);
            //return Ok(mapper.Map<IEnumerable<ArticoliDto>>(articoli));
        }

        [HttpGet("cerca/codice/{CodArt}/{IdList?}", Name = "GetArticoli")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ArticoliDto))]
        public async Task<IActionResult> GetArticoloByCode(string CodArt, string IdList)
        {
            string accessToken = Request.Headers["Authorization"];

            IdList ??= "1";

            if (!await this.articoliRepository.ArticoloExists(CodArt))
            {
                return NotFound(string.Format("Non è stato trovato l'articolo con il codice '{0}'", CodArt));
            }

            var articolo = await this.articoliRepository.SelArticoloByCodice(CodArt);

            PrezziDTO prezzoDTO = await getPriceArtAsync(articolo.CodArt, IdList, accessToken);

            return Ok(CreateArticoloDTO(articolo, prezzoDTO));
        }

        private async Task<PrezziDTO> getPriceArtAsync(string CodArt, string IdList, string Token)
        {
            PrezziDTO prezzo = null;
            using (var client = new HttpClient())
            {
                Token = Token.Replace("Bearer ", "");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                string EndPoitPrezzo = "http://localhost:5003/api/prezzi/";

                var result = await client.GetAsync(EndPoitPrezzo + CodArt + "/" + IdList);

                var response = await result.Content.ReadAsStringAsync();
                prezzo = JsonConvert.DeserializeObject<PrezziDTO>(response);

            }
            return prezzo;
        }

        [HttpGet("cerca/barcode/{Ean}/{IdList?}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ArticoliDto))]
        public async Task<IActionResult> GetArticoloByEan(string Ean, string IdList)
        {
            string accessToken = Request.Headers["Authorization"];

            IdList ??= "1";

            var articolo = await this.articoliRepository.SelArticoloByEan(Ean);

            if (articolo == null)
            {
                return NotFound(string.Format("Non è stato trovato l'articolo con il barcode '{0}'", Ean));
            }

            PrezziDTO prezzoDTO = await getPriceArtAsync(articolo.CodArt, IdList, accessToken);

            return Ok(CreateArticoloDTO(articolo, prezzoDTO));
        }

        [HttpPost("inserisci")]
        [ProducesResponseType(201, Type = typeof(Articoli))]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SaveArticoli([FromBody] Articoli articolo)
        {
            if(articolo == null)
            {
                return BadRequest(ModelState);
            }

            var isPresent = articoliRepository.SelArticoloByCodice2(articolo.CodArt);
            if (isPresent!=null)
            {
                //ModelState.AddModelError("", $"Articolo {articolo.CodArt} presente in anagrafica! Impossibile utilizzare il metodo POST!");
                return StatusCode(422, new InfoMsg(DateTime.Today, $"Articolo {articolo.CodArt} presente in anagrafica! Impossibile utilizzare il metodo POST!"));
            }

            if (!ModelState.IsValid)
            {
                string ErrVal = "";

                foreach(var modelState in ModelState.Values)
                {
                    foreach(var modelError in modelState.Errors)
                    {
                        ErrVal += modelError.ErrorMessage + "|";
                    }
                }

                return BadRequest(new InfoMsg(DateTime.Today, ErrVal));

            }

            articolo.DataCreazione = DateTime.Today;

            if (!await articoliRepository.InsArticoli(articolo))
            {
                ModelState.AddModelError("","Errore generale");
                return StatusCode(500, ModelState);
            }

            //return Ok(CreateArticoloDTO(articolo));
            //return CreatedAtRoute("GetArticoli", new { codart = articolo.CodArt }, CreateArticoloDTO(articolo));
            return Ok(new InfoMsg(DateTime.Today, $"Inserimento articolo {articolo.CodArt} eseguita con successo!"));
        }


        [HttpPut("modifica")]
        [ProducesResponseType(201, Type = typeof(InfoMsg))]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateArticoli([FromBody] Articoli articolo)
        {
            if (articolo == null)
            {
                return BadRequest(ModelState);
            }

            var isPresent = articoliRepository.SelArticoloByCodice2(articolo.CodArt);
            if (isPresent == null)
            {
                ModelState.AddModelError("", "Articolo non presente");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                string ErrVal = "";

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        ErrVal += modelError.ErrorMessage + "|";
                    }
                }

                return BadRequest(new InfoMsg(DateTime.Today, ErrVal));

            }

            if (!await articoliRepository.UpdArticoli(articolo))
            {
                //ModelState.AddModelError("", "Errore generale");
                return StatusCode(422, new InfoMsg(DateTime.Today, $"Articolo {articolo.CodArt} NON presente in anagrafica! Impossibile utilizzare il metodo PUT!"));
            }

            return Ok(new InfoMsg(DateTime.Today, "Articolo modificato con successo."));
        }

        [HttpDelete("elimina/{codart}")]
        [ProducesResponseType(201, Type = typeof(InfoMsg))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        [ProducesResponseType(422, Type = typeof(InfoMsg))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteArticoli(string codart)
        {
            if(codart == "")
            {
                return BadRequest(new InfoMsg(DateTime.Today, "E' necessario inserire il codice dell'articolo da eliminare"));
            }

            var articolo = articoliRepository.SelArticoloByCodice2(codart);

            if (articolo == null)
            {
                return StatusCode(422, new InfoMsg(DateTime.Today, "Articolo non presente"));
            }

            if (!await articoliRepository.DelArticoli(articolo))
            {
                ModelState.AddModelError("", "Errore interno per l'eliminazione dell'articolo");
                return StatusCode(500, ModelState);
            }

            return Ok(new InfoMsg(DateTime.Today, "Articolo cancellato con successo"));
        }

        private ArticoliDto CreateArticoloDTO(Articoli articolo, PrezziDTO prezzo)
        {
            var barcodeDto = new List<BarcodeDto>();

            if (articolo.barcode != null)
            {
                foreach (var ean in articolo.barcode)
                {
                    barcodeDto.Add(new BarcodeDto
                    {
                        Barcode = ean.Barcode,
                        Tipo = ean.IdTipoArt
                    });
                }
            }
            var articoliDto = new ArticoliDto
            {
                CodArt = articolo.CodArt,
                Descrizione = articolo.Descrizione,
                Um = articolo.Um.Trim(),
                CodStat = (articolo.CodStat != null) ? articolo.CodStat.Trim() : "", 
                PzCart = articolo.PzCart,
                PesoNetto = articolo.PesoNetto,
                DataCreazione = articolo.DataCreazione,
                Ean = barcodeDto,
                Categoria = (articolo.famAssort != null) ? articolo.famAssort.Descrizione : "",
                IdStatoArt = (articolo.IdStatoArt != null) ? articolo.IdStatoArt.ToString().Trim() : "",
                IdFamAss = articolo.IdFamAss,
                IdIva = articolo.IdIva,
                Prezzo = prezzo.Prezzo
            };

            return articoliDto;
        }
    }
}