using AlphaShopArticoliAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopArticoliAPI.Services
{
    public interface IArticoliRepository
    {
        Task<ICollection<Articoli>> getArticoli();
        Task<ICollection<Articoli>> SelArticoliByDescrizione(string Descrizione);
        Task<ICollection<Articoli>> SelArticoliByDescrizione(string Descrizione, string IdCat);
        Task<Articoli> SelArticoloByCodice(string Code);
        Articoli SelArticoloByCodice2(string Code);
        Task<Articoli> SelArticoloByEan(string Ean);

        Task<ICollection<Iva>> SelIva();
        Task<ICollection<FamAssort>> SelCat();

        Task<bool> InsArticoli(Articoli articolo);
        Task<bool> UpdArticoli(Articoli articolo);
        Task<bool> DelArticoli(Articoli articolo);
        Task<bool> Salva();

        Task<bool> ArticoloExists(string Code);
    }
}
