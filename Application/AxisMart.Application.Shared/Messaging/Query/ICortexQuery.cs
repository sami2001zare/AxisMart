using AxisMart.Framework;
using MediatR;

namespace AxisMart.Application.Shared.Messaging.Query;

public interface ICortexQuery<TQueryResponse> : IRequest<Result<TQueryResponse>>;
