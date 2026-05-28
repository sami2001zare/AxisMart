using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Application.Ecommerce.User.Manager.Login;

public sealed record LoginManagerCommand(Phone Phone, string Password, bool RememberMe) : ICortexCommand<AccessToken>;
