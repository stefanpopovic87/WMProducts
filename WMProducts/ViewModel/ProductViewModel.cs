using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WMProducts.Models;

namespace WMProducts.ViewModel
{
    public class ProductViewModel
    {
        
        public IEnumerable<Supplier> Suppliers { get; set; }
        public IEnumerable<Manufacturer> Manufacturers { get; set; }
        public IEnumerable<Category> Categories { get; set; }

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

        [Display(Name = "Category")]
        [JsonIgnore]
        public int CategoryId { get; set; }

        [Display(Name = "Manufacturer")]
        [JsonIgnore]
        public int ManufacturerId { get; set; }

        public ProductViewModel()
        {
            Id = 0;
        }


        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            CategoryId = product.CategoryId;
            SupplierId = product.SupplierId;
            ManufacturerId = product.ManufacturerId;
        }


    }
}