using PetStore.Common;
using PetStore.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace PetStore.ServiceModels.Products.InputModels
{
    public class EditProductInputServiceModel
    {
        public string ProductType { get; set; }

        [Required]
        [MinLength(GlobalConstants.UsernameMinLength)]
        [MaxLength(GlobalConstants.UsernameMaxLength)]
        public string Name { get; set; }

        [Range(GlobalConstants.ProductMinPrice, Double.MaxValue)]
        public decimal Price { get; set; }

    }
}
