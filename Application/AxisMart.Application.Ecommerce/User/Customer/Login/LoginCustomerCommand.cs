using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Messaging.Command;

namespace AxisMart.Application.Ecommerce.User.Customer.Login;

public sealed record LoginCustomerCommand(string EmailOrPhone, string Password, bool RememberMe) : ICortexCommand<AccessToken>;
