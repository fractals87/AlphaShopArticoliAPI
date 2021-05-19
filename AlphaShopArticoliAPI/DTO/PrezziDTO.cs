using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopArticoliAPI.Dtos
{
    public class PrezziDTO
    {
        public int Id { get; set; }
        public string Listino { get; set; }
        public decimal Prezzo { get; set; }
    }
}
