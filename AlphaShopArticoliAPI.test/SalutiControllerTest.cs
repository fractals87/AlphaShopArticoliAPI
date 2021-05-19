using AlphaShopArticoliAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace AlphaShopArticoliAPI.test
{
    public class SalutiControllerTest
    {
        SalutiController controller { get;}

        public SalutiControllerTest()
        {

            controller = new SalutiController();
        }

        [Fact]
        public void testGetSaluti()
        {
            var result = controller.getSaluti() as ObjectResult;
            Assert.Equal("\"Saluti, sono il web service MODE\"", result.Value.ToString());
        }

        [Fact]
        public void testGetSaluti2()
        {
            var result = controller.getSaluti2("Paolo") as ObjectResult;
            Assert.Equal("\"Ciao Paolo! esempio con passaggio dati\"", result.Value.ToString());
        }

        [Fact]
        public void testGetSaluti2Err()
        {
            var result = controller.getSaluti2("Marco") as ObjectResult;
            Assert.Equal("\"Utente marco non abilitato\"", result.Value.ToString());
        }
    }
}
