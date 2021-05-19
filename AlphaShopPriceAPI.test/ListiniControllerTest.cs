using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaShopPriceAPI.Controllers;
using AlphaShopPriceAPI.Models;
using AlphaShopPriceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AlphaShopPriceAPI.test
{
    [TestCaseOrderer("AlphaShopPriceAPI.test.AlphabeticalOrderer", "AlphaShopPriceAPI.test")]
    public class ListiniControllerTest
    {
        [Fact]
        public void A_TestSaveListino()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ListiniController(new ListiniRepository(dbContext));

            var Listino = new Listini() { Id = "100", Descrizione = "LISTINO TEST 100", Obsoleto = "No" };

            List<DettListini> dettListini = new List<DettListini>();
            var DettListino = new DettListini { IdList = "100", CodArt = "000028601", Prezzo = 2 };
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
        public void B_TestErrSaveListino()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ListiniController(new ListiniRepository(dbContext));

            var Listino = new Listini() { Id = "100", Descrizione = "LISTINO TEST 100", Obsoleto = "No" };

            // Act
            var response = controller.SaveListino(Listino) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(422, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Listino 100 presente in anagrafica! Impossibile utilizzare il metodo POST!", value.Message);

        }

        [Fact]
        public void C_TestUpdateListino()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ListiniController(new ListiniRepository(dbContext));

            var Listino = new Listini() { Id = "100", Descrizione = "LISTINO TEST 100", Obsoleto = "No" };

            List<DettListini> dettListini = new List<DettListini>();
            var DettListino = new DettListini { IdList = "100", CodArt = "000035901", Prezzo = 1.90M };
            dettListini.Add(DettListino);

            Listino.DettListini = dettListini;

            // Act
            var response = controller.UpdateListino(Listino) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Modifica listino 100 eseguita con successo!", value.Message);

        }

        [Fact]
        public void D_TestErrUpdateListino()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ListiniController(new ListiniRepository(dbContext));

            var Listino = new Listini() { Id = "101", Descrizione = "LISTINO TEST 100", Obsoleto = "No" };

            List<DettListini> dettListini = new List<DettListini>();
            var DettListino = new DettListini { IdList = "101", CodArt = "000035901", Prezzo = 1.90M };
            dettListini.Add(DettListino);

            Listino.DettListini = dettListini;

            // Act
            var response = controller.UpdateListino(Listino) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(422, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Listino 101 NON presente in anagrafica! Impossibile utilizzare il metodo PUT!", value.Message);

        }

        [Fact]
        public async Task E_TestGetListById()
        {

            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ListiniController(new ListiniRepository(dbContext));

            // Act
            var response = await controller.GetListById("100") as ObjectResult;
            var value = response.Value as Listini;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("LISTINO TEST 100", value.Descrizione);
        }

        [Fact]
        public void F_TestDelListino()
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

        [Fact]
        public void G_TestErrDelListino()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ListiniController(new ListiniRepository(dbContext));

            // Act
            var response = controller.DeleteListino("100") as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(422, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Listino 100 NON presente in anagrafica! Impossibile Eliminare!", value.Message);

        }


    }
}