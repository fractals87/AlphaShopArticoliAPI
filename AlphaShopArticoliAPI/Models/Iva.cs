using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopArticoliAPI.Models
{
    public class Iva
    {
        [Key]
        public int IdIva { get; set; }
        public string Descrizione { get; set; }
        [Required]
        public Int16 Aliquota { get; set; }

        public virtual ICollection<Articoli> Articoli { get; set; }
    }
}
