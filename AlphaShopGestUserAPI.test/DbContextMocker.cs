using AlphaShopGestUserAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaShopArticoliAPI.test
{
    public class DbContextMocker
    {
        public static AlphaShopDbContext alphaShopDbContext()
        {
            var connectionString = "Server=3P-NB001\\SQLEXPRESS;Database=AlphaShop;Trusted_Connection=True;MultipleActiveResultSets=true";

            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<AlphaShopDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Create instance of DbContext
            var dbContext = new AlphaShopDbContext(options);

            return dbContext;
        }
    }
}
