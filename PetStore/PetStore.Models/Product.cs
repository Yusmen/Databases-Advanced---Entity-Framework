﻿using PetStore.Common;
using PetStore.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public ProductType ProductType { get; set; }
        [Required]
        [MinLength(GlobalConstants.UsernameMinLength)]
        public string Name { get; set; }

        [Range(GlobalConstants.ProductMinPrice, Double.MaxValue)]
        public decimal Price { get; set; }

    }
}
