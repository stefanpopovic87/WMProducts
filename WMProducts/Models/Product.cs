﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WMProducts.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2 and 50 characters")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Must be maximum 255 characters")]
        public string Description { get; set; }

        [Required]
        [Range(0, 99999.99)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public decimal Price { get; set; }

        [Display(Name = "Supplier")]
        [JsonIgnore]
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        [Display(Name = "Category")]
        [JsonIgnore]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Display(Name = "Manufacturer")]
        [JsonIgnore]
        public int ManufacturerId { get; set; }

        public Manufacturer Manufacturer { get; set; }

        //public virtual Supplier Supplier { get; set; }
        //public virtual Category Category { get; set; }
        //public virtual Manufacturer Manufacturer { get; set; }

        //public virtual Store Store { get; set; }
    }
}