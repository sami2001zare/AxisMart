using AxisMart.Framework;
using MediatR;

namespace AxisMart.Application.Shared.Messaging.Query;

public interface ICortexQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : ICortexQuery<TResponse>;
