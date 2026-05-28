using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Application.Ecommerce.User.Customer.ValidateLoginWithOTP;

public sealed record CustomerLoginVaidateWithOTPCommand(Phone Phone, string OTP) : ICortexCommand<string>;
