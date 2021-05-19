using AlphaShopArticoliAPI.Controllers;
using AlphaShopArticoliAPI.DTO;
using AlphaShopArticoliAPI.Models;
using AlphaShopArticoliAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AlphaShopArticoliAPI.test
{
    public class ArticoliControllerTestIns
    {
        private Articoli CreateArtTest()
        {
            var Articolo = new Articoli()
            {
                CodArt = "67000023",
                Descrizione = "TEST DESC",
                Um = "PZ",
                CodStat = "TESTART",
                PzCart = 6,
                PesoNetto = 1,
                IdIva = 10,
                IdFamAss = 1,
                IdStatoArt = "1",
                DataCreazione = DateTime.Today
            };

            List<Ean> Barcodes = new List<Ean>();
            var Barcode = new Ean { CodArt = "123Test", Barcode = "5548526", IdTipoArt = "CP" };
            Barcodes.Add(Barcode);

            Articolo.barcode = Barcodes;

            return Articolo;
        }

        private Articoli CreateArtTest2()
        {
            var Articolo = new Articoli()
            {
                CodArt = "123Test",
                Descrizione = "TEST DESC",
                Um = "PZ",
                CodStat = "TESTART",
                PzCart = 600,
                PesoNetto = 1,
                IdIva = 10,
                IdFamAss = 1,
                IdStatoArt = "1",
                DataCreazione = DateTime.Today
            };

            List<Ean> Barcodes = new List<Ean>();
            var Barcode = new Ean { CodArt = "123Test", Barcode = "5548526", IdTipoArt = "CP" };
            Barcodes.Add(Barcode);

            Articolo.barcode = Barcodes;

            return Articolo;
        }

        [Fact]
        public async Task ATestSaveArticolo()
        {

            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            // Act
            var response = await controller.SaveArticoli(this.CreateArtTest()) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Inserimento articolo 123Test eseguita con successo!", value.Message);
        }

        [Fact]
        public async Task BTestSaveErrArticolo()
        {

            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            // Act
            var response = await controller.SaveArticoli(this.CreateArtTest()) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(422, response.StatusCode);
            //System.Diagnostics.Debug.WriteLine("ciao" + value);
            Assert.NotNull(value);
            Assert.Equal("Articolo 123Test presente in anagrafica! Impossibile utilizzare il metodo POST!", value.Message);
        }

        [Fact]
        public async Task CTestUpdArticolo()
        {

            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            Articoli articolo = this.CreateArtTest2();
            articolo.Descrizione = "DESCRIZIONE MODIFICATA";

            // Act
            var response = await controller.UpdateArticoli(this.CreateArtTest2()) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Articolo modificato con successo.", value.Message);
        }

        [Fact]
        public async Task DTestDeletedArticolo()
        {

            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            // Act
            var response = await controller.DeleteArticoli("123Test") as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Articolo cancellato con successo", value.Message);
        }
    }
}
