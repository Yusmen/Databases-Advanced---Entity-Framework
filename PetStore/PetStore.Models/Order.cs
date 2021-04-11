﻿using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PetStore.Models
{
    public class Order
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ClientProducts = new HashSet<ClientProduct>();
        }
        [Key]
        public string Id { get; set; }

        [Required]
        [MinLength(GlobalConstants.TownMinLength)]
        public string Town { get; set; }
        [Required]
        [MinLength(GlobalConstants.AddressMinLength)]
        public string Address { get; set; }

        public string Notes { get; set; }

        public virtual ICollection<ClientProduct> ClientProducts { get; set; }

        public decimal TotalPrice 
            => this.ClientProducts.Sum(cp => cp.Product.Price * cp.Quantity);

    }
}
