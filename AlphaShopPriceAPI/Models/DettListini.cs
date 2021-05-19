using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopPriceAPI.Models
{
    public class DettListini
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdList { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Il numero minimo di caratteri è 5")]
        [MaxLength(30, ErrorMessage = "Il numero massimo di caratteri è 30")]
        public string CodArt { get; set; }

        [Required]
        [Range(0.01, 1000, ErrorMessage = "Il prezzo deve avere un valore minimo di 1 centesimo e massimo di 1000 euro")]
        public decimal Prezzo { get; set; }

        public virtual Listini Listino { get; set; }

    }
}
