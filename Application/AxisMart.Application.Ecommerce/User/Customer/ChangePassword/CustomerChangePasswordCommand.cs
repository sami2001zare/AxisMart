using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Application.Ecommerce.User.Customer.ChangePassword;

public sealed record CustomerChangePasswordCommand(Phone Phone, string NewPassword) : ICortexCommand<bool>;
