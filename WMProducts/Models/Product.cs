using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WMProducts.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        //public virtual Supplier Supplier { get; set; }
        //public virtual Category Category { get; set; }
        //public virtual Manufacturer Manufacturer { get; set; }

        //public virtual Store Store { get; set; }
    }
}