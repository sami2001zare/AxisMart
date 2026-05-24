using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AxisMart.Infra.Ecommerce.Repositories.User;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .HasConversion(firstName => firstName.Value, value => new FirstName(value));

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .HasConversion(lastName => lastName.Value, value => new LastName(value));

        builder.Property(x => x.Phone)
            .HasMaxLength(25)
            .HasConversion(phone => phone.Value, value => new Phone(value));

        builder.Property(x => x.Email)
            .HasMaxLength(25)
            .HasConversion(email => email.Value, value => new Email(value));
    }
}
