using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopPriceAPI.Models
{
    public class Listini
    {
        [Key]
        [MinLength(1, ErrorMessage = "Inserisci l'Id del Listino")]
        public string Id { get; set; }

        [MinLength(5, ErrorMessage = "La Descrizione deve avere almeno 5 caratteri")]
        [MaxLength(30, ErrorMessage = "La Descrizione non può essere più lunga di 30 caratteri")]
        public string Descrizione { get; set; }

        public string Obsoleto { get; set; }

        public virtual ICollection<DettListini> DettListini { get; set; }
    }
}
