using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AlphaShopArticoliAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalutiController : ControllerBase
    {
        [HttpGet]
        public IActionResult getSaluti()
        {
            return Ok("\"Saluti, sono il web service MODE\"");
        }

        [HttpGet("{Nome}")]
        public IActionResult getSaluti2(string Nome)
        {
            try
            {
                if (Nome == "Marco")
                    throw new Exception("\"Utente marco non abilitato\"");
                else
                    return Ok(string.Format("\"Ciao {0}! esempio con passaggio dati\"", Nome));
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }

        }
    }
}