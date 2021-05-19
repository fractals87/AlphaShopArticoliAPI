using AlphaShopPriceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopPriceAPI.Services
{
    public interface IPrezziRepository
    {
        bool PrezzoExists(string CodArt, string IdListino);
        Task<DettListini> SelPrezzoByCodArtAndList(string CodArt, string IdListino);
        Task<bool> DelPrezzoListino(string CodArt, string IdListino);
    }
}
