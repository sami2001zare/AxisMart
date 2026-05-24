using AxisMart.Infra.Ecommerce.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AxisMart.Infra.Ecommerce.Repositories.User;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("Outbox_Messages");

        builder.HasKey(om => om.Id);
    }
}
