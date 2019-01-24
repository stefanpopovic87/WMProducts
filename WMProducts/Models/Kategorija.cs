using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WMProducts.Models
{
    [Table("KategorijeDb")]
    public class Kategorija
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Mora imati između 2 i 100 karaktera")]
        public string Naziv { get; set; }
    }
}