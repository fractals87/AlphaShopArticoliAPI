using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopArticoliAPI.Models
{
    public class Ean
    {
        public string CodArt { get; set; }
        [Key]
        [StringLength(13, MinimumLength = 8, ErrorMessage = "Il Barcode deve avere da 8 a 13 cifre")]
        public string Barcode { get; set; }
        [Required]
        public string IdTipoArt { get; set; }

        public virtual Articoli articolo { get; set; }


    }
}
