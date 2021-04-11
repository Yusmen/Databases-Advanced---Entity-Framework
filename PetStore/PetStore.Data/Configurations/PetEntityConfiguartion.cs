using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Common;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Data.Configurationss
{
    public class PetEntityConfiguartion : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(GlobalConstants.PetNameMaxLength)
                .IsUnicode(true);
        }
    }
}
