using AxisMart.Application.Shared.Authentication;
using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;

namespace AxisMart.Infra.Ecommerce.Repositories.User;

internal sealed class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
{
    public void Configure(EntityTypeBuilder<Administrator> builder)
    {
        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .HasConversion(firstName => firstName.Value, value => new FirstName(value));

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .HasConversion(lastName => lastName.Value, value => new LastName(value));

        builder.Property(x => x.UserName)
            .HasMaxLength(100)
            .HasConversion(lastName => lastName.Value, value => new UserName(value));

        builder.Property(x => x.Phone)
            .HasMaxLength(25)
            .HasConversion(phone => phone.Value, value => new Phone(value));

        builder.Property(x => x.Email)
            .HasMaxLength(25)
            .HasConversion(email => email.Value, value => new Email(value));

        builder.HasData([
            Administrator.Create(Guid.Parse("019e760f-c583-7dc7-a590-f6b2e09ab5ad"), new FirstName("رسول"), new LastName("طاهری"), new UserName("r.taheri"), new Email("r.taheri@gmail.com"), new Phone("09123456789")),
            Administrator.Create(Guid.Parse("019e760f-c583-70c3-adb6-8d91e2fa3422"), new FirstName("سامان"), new LastName("زارع"), new UserName("s.zare"), new Email("saman.zare@modare.ac"), new Phone("09121039846"))
        ]);
    }
}
