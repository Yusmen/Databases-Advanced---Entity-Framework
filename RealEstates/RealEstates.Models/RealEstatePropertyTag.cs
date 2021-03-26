using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Models
{
    public class RealEstatePropertyTag
    {

        public int PropertyId { get; set; }

        public RealEstateProperty Property { get; set; }

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; } 
    }
}
