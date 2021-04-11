using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Common;
using PetStore.Models;

namespace PetStore.Data.Configurations
{
    public class TownEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .Property(x => x.Town)
                .HasMaxLength(GlobalConstants.TownMaxLength)
                .IsUnicode(true);

            builder
                .Property(a => a.Address)
                .HasMaxLength(GlobalConstants.AddressMaxLength)
                .IsUnicode(true);

            builder
                .Ignore(o => o.TotalPrice);
        }
    }
}
