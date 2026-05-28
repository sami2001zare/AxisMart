using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Application.Ecommerce.User.Manager.ForgotPassword;

public sealed record ManagerForgotPasswordCommand(Guid Id) : ICortexCommand<string>;
