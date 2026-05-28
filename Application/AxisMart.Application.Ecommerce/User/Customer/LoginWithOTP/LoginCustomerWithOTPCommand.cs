using AxisMart.Application.Shared.Messaging.Command;

namespace AxisMart.Application.Ecommerce.User.Customer.LoginWithOTP;

public sealed record LoginCustomerWithOTPCommand(string EmailOrPhone) : ICortexCommand;
