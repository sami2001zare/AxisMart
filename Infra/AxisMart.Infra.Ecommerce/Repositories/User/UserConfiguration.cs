using AxisMart.Core.Ecommerce.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AxisMart.Infra.Ecommerce.Repositories.User;

internal sealed class UserConfiguration : IEntityTypeConfiguration<Core.Ecommerce.User.User>
{
    public void Configure(EntityTypeBuilder<Core.Ecommerce.User.User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .HasConversion(firstName => firstName.Value, value => new FirstName(value));

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .HasConversion(lastName => lastName.Value, value => new LastName(value));

        builder.Property(x => x.Phone)
            .HasMaxLength(25)
            .HasConversion(phone => phone.Value, value => new Phone(value));

    }
}
