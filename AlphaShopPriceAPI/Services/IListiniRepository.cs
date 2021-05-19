using AlphaShopPriceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopPriceAPI.Services
{
    public interface IListiniRepository
    {
        Task<Listini> SelById(string Id);
        Listini SelByIdNoTrack(string Id);
        Listini CheckListino(string Id);
        bool InsListini(Listini listino);
        bool UpdListini(Listini listino);
        bool DelListini(Listini listino);
        bool Salva();
    }
}
