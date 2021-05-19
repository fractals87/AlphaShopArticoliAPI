using AlphaShopPriceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopPriceAPI.Services
{
    public class AlphaShopDbContext : DbContext
    {
        public AlphaShopDbContext(DbContextOptions<AlphaShopDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Listini> Listini { get; set; }
        public virtual DbSet<DettListini> DettListini { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Listini>()
            .HasKey(a => new { a.Id });

            modelBuilder.Entity<DettListini>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();


            //Relazione one to many (uno a molti) fra listini e dettListini
            modelBuilder.Entity<DettListini>()
                .HasOne<Listini>(s => s.Listino) //ad un listino...
                .WithMany(g => g.DettListini) //corrispondono molti dettaglio listino
                .HasForeignKey(s => s.IdList); //la chiave esterna dell'entity DettListini



        }
    }
}
