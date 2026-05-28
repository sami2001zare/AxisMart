using AxisMart.Application.Shared.Messaging.Command;

namespace AxisMart.Application.Ecommerce.User.Manager.Logout;

public sealed record LogoutAdminCommand : ICortexCommand<bool>;
