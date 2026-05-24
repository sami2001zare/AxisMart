using AxisMart.Framework;
using MediatR;

namespace AxisMart.Application.Shared.Messaging.Command;

public interface ICortexCommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICortexCommand;

public interface ICortexCommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICortexCommand<TResponse>;