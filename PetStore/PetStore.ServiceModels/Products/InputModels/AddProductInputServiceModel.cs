﻿using PetStore.Common;
using PetStore.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace PetStore.ServiceModels.Products.InputModels
{
    public class AddProductInputServiceModel
    {

       

        [Required]
        [MinLength(GlobalConstants.UsernameMinLength)]
        [MaxLength(GlobalConstants.UsernameMaxLength)]

       
        public string Name { get; set; }

        public int ProductType { get; set; }

        [Range(GlobalConstants.ProductMinPrice, Double.MaxValue)]
        public decimal Price { get; set; }


    }
}
