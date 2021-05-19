using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopArticoliAPI.Models
{
    public class FamAssort
    {
        [Key]
        public int Id { get; set; }
        public string Descrizione { get; set; }

        public virtual ICollection<Articoli> Articoli { get; set; }
    }
}
