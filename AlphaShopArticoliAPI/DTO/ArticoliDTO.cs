using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopArticoliAPI.DTO
{
    public class ArticoliDto
    {
        public string CodArt { get; set; }
        public string Descrizione { get; set; }
        public string Um { get; set; }
        public string CodStat { get; set; }
        public int? PzCart { get; set; }
        public double? PesoNetto { get; set; }
        public DateTime? DataCreazione { get; set; }

        public string IdStatoArt { get; set; }

        public ICollection<BarcodeDto> Ean { get; set; }

        public int? IdFamAss { get; set; }
        public int? IdIva { get; set; }

        public string Categoria { get; set; }

        public decimal Prezzo { get; set; }
    }
}
