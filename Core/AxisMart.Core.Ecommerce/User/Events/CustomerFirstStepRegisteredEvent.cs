using AxisMart.Core.Ecommerce.User.ValueObjects;
using AxisMart.Framework;

namespace AxisMart.Core.Ecommerce.User.Events;

public sealed record CustomerFirstStepRegisteredEvent(Phone Phone) : IDomainEvent;
