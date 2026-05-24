using AxisMart.Application.Shared.Clock;
using AxisMart.Core.Ecommerce.User;
using AxisMart.Framework;
using AxisMart.Framework.Repository;
using AxisMart.Infra.Ecommerce.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace AxisMart.Infra.Ecommerce;

public sealed class AxisMartContext : DbContext, IUnitOfWork
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };

    private readonly IDateTimeProvider _dateTimeProvider;

    public AxisMartContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Credential> Credentials { get; set; }
    public DbSet<JsonWebToken> JsonWebTokens { get; set; }
    
    public DbSet<OneTimePassword> OTPs { get; set; }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await Database.BeginTransactionAsync(cancellationToken);

        try
        {
            //AddDomainEventAsOutboxMessages(cancellationToken);
            AddDomainEventAsOutboxMessages();

            int result = await base.SaveChangesAsync(cancellationToken);
            await Database.CommitTransactionAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException)
        {
            await Database.RollbackTransactionAsync(cancellationToken);
            throw;
        }
        catch (Exception)
        {
            await Database.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public override EntityEntry Update(object entity)
    {
        return base.Update(entity);
    }

    public override EntityEntry Remove(object entity)
    {
        return base.Remove(entity);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AxisMartContext).Assembly);

        modelBuilder.Entity<User>()
            .UseTptMappingStrategy();

        base.OnModelCreating(modelBuilder);
    }

    //private void AddDomainEventAsOutboxMessages(CancellationToken cancellationToken = default)
    private void AddDomainEventAsOutboxMessages()
    {
        var outboxMessages = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = entity.GetDomainEvents();

                return domainEvents;

                entity.ClearDomainEvents();
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.CreateVersion7(),
                _dateTimeProvider.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)))
            .ToList();


        AddRange(outboxMessages);
    }
}