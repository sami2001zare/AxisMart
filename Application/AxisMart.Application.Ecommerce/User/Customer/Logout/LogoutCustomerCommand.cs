using AxisMart.Application.Shared.Messaging.Command;

namespace AxisMart.Application.Ecommerce.User.Customer.Logout;

public sealed record LogoutCustomerCommand : ICortexCommand<bool>;
