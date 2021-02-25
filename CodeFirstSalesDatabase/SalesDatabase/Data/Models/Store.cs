using SalesDatabase.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SalesDatabase.Data.Models
{
    public class Store
    {
        public Store()
        {
            this.Sales = new HashSet<Sale>();
        }

        [Key]
        public int StoreId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.StoreNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }


    }
}
