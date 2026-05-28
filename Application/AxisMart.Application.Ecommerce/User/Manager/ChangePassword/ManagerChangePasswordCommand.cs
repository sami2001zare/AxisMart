using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Application.Ecommerce.User.Manager.ChangePassword;

public sealed record ManagerChangePasswordCommand(Guid Id, string NewPassword) : ICortexCommand<bool>;
