using AlphaShopPriceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopPriceAPI.Services
{
    public class ListiniRepository : IListiniRepository
    {
        private AlphaShopDbContext alphaShopDbContext;

        public ListiniRepository(AlphaShopDbContext alphaShopDbContext)
        {
            this.alphaShopDbContext = alphaShopDbContext;
        }

        public async Task<Listini> SelById(string Id)
        {
            return await this.alphaShopDbContext.Listini
                .Include(r => r.DettListini)
                .Where(a => a.Id.Equals(Id))
                .FirstOrDefaultAsync();
        }

        public Listini SelByIdNoTrack(string Id)
        {
            return this.alphaShopDbContext.Listini
                .AsNoTracking()
                .Include(r => r.DettListini)
                .Where(a => a.Id.Equals(Id))
                .FirstOrDefault();
        }

        public Listini CheckListino(string Id)
        {
            return this.alphaShopDbContext.Listini
                //.Include(r => r.DettListini)
                .AsNoTracking()
                .Where(a => a.Id.Equals(Id))
                .FirstOrDefault();
        }
        public bool InsListini(Listini listino)
        {
            this.alphaShopDbContext.Add(listino);
            return Salva();
        }

        public bool UpdListini(Listini listino)
        {
            this.alphaShopDbContext.Update(listino);
            return Salva();
        }

        public bool DelListini(Listini listino)
        {
            this.alphaShopDbContext.Remove(listino);
            return Salva();
        }

        public bool Salva()
        {
            var saved = this.alphaShopDbContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
