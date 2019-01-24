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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.###}")]
        public decimal Cena { get; set; }

        [JsonIgnore]
        [Display(Name = "Dobavljač")]
        public int DobavljačId { get; set; }

        [JsonIgnore]
        [Display(Name = "Kategorija")]
        public int KategorijaId { get; set; }

        [JsonIgnore]
        [Display(Name = "Proizvođač")]
        public int ProizvođačId { get; set; }

        //public virtual Supplier Supplier { get; set; }
        //public virtual Category Category { get; set; }
        //public virtual Manufacturer Manufacturer { get; set; }

        //public virtual Store Store { get; set; }
    }
}