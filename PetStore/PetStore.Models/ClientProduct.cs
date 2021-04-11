using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PetStore.Models
{
    public class ClientProduct
    {
        [Required]
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }

        public Client Client { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Range(1, Int32.MaxValue)]
        public int Quantity { get; set; }
    }
}
