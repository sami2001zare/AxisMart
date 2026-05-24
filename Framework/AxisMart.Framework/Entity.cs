using System.ComponentModel.DataAnnotations;

namespace AxisMart.Framework
{
    public class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        protected Entity(Guid id)
        {
            Id = id;
        }

        protected Entity()
        {
        }

        public Guid Id { get; init; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents;
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
