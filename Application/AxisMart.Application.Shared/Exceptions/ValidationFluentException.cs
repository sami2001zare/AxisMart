namespace AxisMart.Application.Shared.Exceptions;

public sealed class ValidationFluentException(IEnumerable<ValidationError> errors) : ApplicationException
{
    public IEnumerable<ValidationError> Errors { get; } = errors;
}
