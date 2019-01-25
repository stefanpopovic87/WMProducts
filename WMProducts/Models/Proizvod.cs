using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WMProducts.Models
{
    [Table("ProizvodiDb")]
    public class Proizvod
    {

        public Dobavljač Dobavljač { get; set; }
        public Kategorija Kategorija { get; set; }
        public Proizvođač Proizvođač { get; set; }

        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Mora imati između 2 i 50 karaktera")]
        public string Naziv { get; set; }

        [StringLength(255, ErrorMessage = "Može imati maksimalno 255 karaktera")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [Range(1, 9999999.99, ErrorMessage = "Cena mora biti u opsegu od 1 do 9999999.99 RSD")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Pogrešan unos")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Cena { get; set; }

        [JsonIgnore]
        [Display(Name = "Dobavljač")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public int DobavljačId { get; set; }

        [JsonIgnore]
        [Display(Name = "Kategorija")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public int KategorijaId { get; set; }

        [JsonIgnore]
        [Display(Name = "Proizvođač")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public int ProizvođačId { get; set; }
    }
}