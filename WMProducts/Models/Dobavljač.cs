using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WMProducts.Models
{
    [Table("DobavljačiDb")]
    public class Dobavljač
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Mora imati između 2 i 100 karaktera")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Polje je obavezno.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Koristiti samo brojeve")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Mora imati 9 brojeva.")]
        [DisplayName("PIB")]
        public string Pib { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Mora imati između 10 i 100 karaktera.")]
        public string Adresa { get; set; }
    }
}