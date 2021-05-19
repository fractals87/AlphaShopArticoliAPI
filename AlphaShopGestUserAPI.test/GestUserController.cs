using AlphaShopGestUserAPI.Helper;
using AlphaShopGestUserAPI.Service;
using GestUser.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AlphaShopArticoliAPI.test
{
    public class ArticoliControllerTest
    {
        IOptions<AppSettings> AppSettings = Options.Create<AppSettings>(new AppSettings() { Secret = "$secretPasswod1!" });

        [Fact]
        public async Task A_TestGetAllUser()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new UserController(new UserService(dbContext, AppSettings));

            // Act
            var response = await controller.GetAllUser() as ObjectResult;
            var value = response.Value as ICollection<Utenti>;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
        }

        private Utenti CreateUtenteTest()
        {
            var utente = new Utenti()
            {
                CodFidelity = "67000023",
                UserId = "Paolo",
                Password = "Password11",
                Abilitato = "SI"
            };

            List<Profili> Profili = new List<Profili>();
            Profili.Add(new Profili { CodFidelity = "67000023",  Tipo = "USER" });
            Profili.Add(new Profili { CodFidelity = "67000023", Tipo = "ADMIN" });

            utente.Profili = Profili;

            return utente;
        }

        [Fact]
        private void B_TestIns()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new UserController(new UserService(dbContext, AppSettings));

            // Act
            var response = controller.SaveUtente(CreateUtenteTest()) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Inserimento Utente Paolo eseguito con successo!", value.Message);
        }

        [Fact]
        private void C_TestDel()
        {
            // Arrange
            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new UserController(new UserService(dbContext, AppSettings));

            // Act
            var response = controller.DeleteUser("Paolo") as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Eliminazione utente Paolo eseguita con successo!", value.Message);
        }

        //[Fact]
        //public async Task TestErrGetArticoloByCode()
        //{
        //    string CodArt = "0009926010";

        //    // Arrange
        //    var dbContext = DbContextMocker.alphaShopDbContext();
        //    var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

        //    // Act
        //    var response = await controller.GetArticoloByCode(CodArt) as ObjectResult;
        //    var value = response.Value as ArticoliDto;

        //    dbContext.Dispose();

        //    // Assert
        //    Assert.Equal(404, response.StatusCode);
        //    Assert.Null(value);
        //    Assert.Equal("Non è stato trovato l'articolo con il codice '0009926010'", response.Value);
        //}

        //[Fact]
        //public async Task TestSelArticoliByDescrizione()
        //{
        //    string Descrizione = "ACQUA ROCCHETTA";

        //    // Arrange
        //    var dbContext = DbContextMocker.alphaShopDbContext();
        //    var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

        //    // Act
        //    var actionResult = await controller.GetArticoliByDesc(Descrizione,"");
        //    //var value = response.Value as ICollection<ArticoliDto>;

        //    dbContext.Dispose();

        //    // Assert
        //    var result = actionResult.Result as ObjectResult;
        //    var value = result.Value as ICollection<ArticoliDto>;
        //    Assert.Equal(200, result.StatusCode);
        //    Assert.NotNull(value);
        //    Assert.Equal(3, value.Count);
        //    Assert.Equal("002001201", value.FirstOrDefault().CodArt);
        //}

        //[Fact]
        //public async Task TestErrSelArticoliByDescrizione()
        //{
        //    string Descrizione = "PIPPO";

        //    // Arrange
        //    var dbContext = DbContextMocker.alphaShopDbContext();
        //    var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

        //    // Act
        //    var actionResult = await controller.GetArticoliByDesc(Descrizione,"1");
        //    dbContext.Dispose();

        //    //Assert
        //    var result = actionResult.Result as ObjectResult;
        //    var value = actionResult.Value as ICollection<ArticoliDto>;
        //    Assert.Equal(404, result.StatusCode);
        //    Assert.Null(value);
        //    Assert.Equal("Nessun articolo trovato.", result.Value);

        //}

        //[Fact]
        //public async Task TestSelArticoloByEan()
        //{
        //    string Ean = "80582533";

        //    // Arrange
        //    var dbContext = DbContextMocker.alphaShopDbContext();
        //    var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

        //    // Act
        //    var response = await controller.GetArticoloByEan(Ean) as ObjectResult;
        //    var value = response.Value as ArticoliDto;

        //    dbContext.Dispose();

        //    // Assert
        //    Assert.Equal(200, response.StatusCode);
        //    Assert.NotNull(value);
        //    Assert.Equal("000974302", value.CodArt);

        //}

        //[Fact]
        //public async Task TestErrSelArticoloByEan()
        //{
        //    string Ean = "80582533A";

        //    // Arrange
        //    var dbContext = DbContextMocker.alphaShopDbContext();
        //    var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

        //    // Act
        //    var response = await controller.GetArticoloByEan(Ean) as ObjectResult;
        //    var value = response.Value as ArticoliDto;

        //    dbContext.Dispose();

        //    // Assert
        //    Assert.Equal(404, response.StatusCode);
        //    Assert.Null(value);
        //    Assert.Equal("Non è stato trovato l'articolo con il barcode '80582533A'", response.Value);
        //}
    }
}
