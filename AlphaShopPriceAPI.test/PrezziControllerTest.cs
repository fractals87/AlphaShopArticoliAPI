using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaShopPriceAPI.Controllers;
using AlphaShopPriceAPI.Dtos;
using AlphaShopPriceAPI.Models;
using AlphaShopPriceAPI.Services;
using AlphaShopPriceAPI.test;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AlphaShopArticoliAPI.test
{
    [TestCaseOrderer("AlphaShopPriceAPI.Test.AlphabeticalOrderer", "AlphaShopPriceAPI.test")]
    public class PrezziControllerTest
    {
        [Fact]
        public void A_TestSaveListino()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ListiniController(new ListiniRepository(dbContext));

            var Listino = new Listini() { Id = "100", Descrizione = "LISTINO TEST 100", Obsoleto = "No" };

            List<DettListini> dettListini = new List<DettListini>();
            var DettListino = new DettListini { IdList = "100", CodArt = "000028601", Prezzo = 2.99M };
            dettListini.Add(DettListino);

            Listino.DettListini = dettListini;

            // Act
            var response = controller.SaveListino(Listino) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Inserimento listino 100 eseguito con successo!", value.Message);

        }

        [Fact]
        public async Task B_TestGetPriceCodArt()
        {

            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new PrezziController(new PrezziRepository(dbContext));

            // Act
            var response = await controller.GetPriceCodArt("000028601", "100") as ObjectResult;
            var value = response.Value as PrezziDTO;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal(2.99M, value.Prezzo);
            Assert.Equal("100", value.Listino);
        }

        [Fact]
        public async Task C_TestDelPrezzo()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new PrezziController(new PrezziRepository(dbContext));

            // Act
            var response = await controller.DeletePrice("100", "000028601") as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Eliminazione del prezzo Listino 100 del codice 000028601 eseguita con successo!", value.Message);

        }

        [Fact]
        public void D_TestDelListino()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ListiniController(new ListiniRepository(dbContext));

            // Act
            var response = controller.DeleteListino("100") as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Eliminazione Listino 100 eseguita con successo!", value.Message);

        }


    }
}