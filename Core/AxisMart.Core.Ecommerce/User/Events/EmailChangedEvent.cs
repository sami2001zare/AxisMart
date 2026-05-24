using AxisMart.Core.Ecommerce.User.ValueObjects;
using AxisMart.Framework;

namespace AxisMart.Core.Ecommerce.User.Events;


public sealed record EmailChangedEvent() : IDomainEvent;

public sealed record CustomerFirstStepRegisteredEvent(Phone Phone) : IDomainEvent;

public sealed record LoggedInEvent() : IDomainEvent;

public sealed record LoggedOutEvent() : IDomainEvent;

public sealed record PhoneNumberChangedEvent() : IDomainEvent;