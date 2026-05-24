using AxisMart.Framework;
using MediatR;

namespace AxisMart.Application.Shared.Messaging.Command;

public interface ICortexCommand : IRequest<Result>, IBaseCommand;

public interface ICortexCommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;
