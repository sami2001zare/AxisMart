using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Application.Ecommerce.User.Customer.ValidateRegistration;

public sealed record CustomerVaidateRegisterationCommand(Phone Phone, string OTP) : ICortexCommand<string>;
