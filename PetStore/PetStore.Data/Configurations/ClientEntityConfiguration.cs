using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Common;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Data.Configurations
{
    public class ClientEntityConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(x => x.Username)
                .HasMaxLength(GlobalConstants.UsernameMaxLength)
                .IsUnicode(false);

            builder
                .Property(e => e.Email)
                .HasMaxLength(GlobalConstants.EmailMaxLength)
                .IsUnicode(false);

            builder
                .Property(n => n.FirstName)
                .HasMaxLength(GlobalConstants.ClientNameMaxLength)
                .IsUnicode(true);

            builder
               .Property(n => n.LastName)
               .HasMaxLength(GlobalConstants.ClientNameMaxLength)
               .IsUnicode(true);
        }
    }
}
