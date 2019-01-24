using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WMProducts.Models;

namespace WMProducts.ViewModel
{
    public class ProizvodViewModel
    {

        public IEnumerable<Dobavljač> Dobavljači { get; set; }
        public IEnumerable<Proizvođač> Proizvođači { get; set; }
        public IEnumerable<Kategorija> Kategorije { get; set; }

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
        [Required(ErrorMessage = "Polje je obavezno")]
        [Display(Name = "Dobavljač")]
        public int DobavljačId { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Polje je obavezno")]
        [Display(Name = "Kategorija")]
        public int KategorijaId { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Polje je obavezno")]
        [Display(Name = "Proizvođač")]
        public int ProizvođačId { get; set; }

        public ProizvodViewModel()
        {
            Id = 0;
        }


        public ProizvodViewModel(Proizvod proizvod)
        {
            Id = proizvod.Id;
            Naziv = proizvod.Naziv;
            Opis = proizvod.Opis;
            Cena = proizvod.Cena;
            KategorijaId = proizvod.KategorijaId;
            DobavljačId = proizvod.DobavljačId;
            ProizvođačId = proizvod.ProizvođačId;
        }


    }
}