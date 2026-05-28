using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Application.Ecommerce.User.Customer.ForgotPassword;

public sealed record CustomerForgotPasswordCommand(Phone Phone) : ICortexCommand<string>;
