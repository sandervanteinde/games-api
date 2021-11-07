using GameApis.Domain.Exceptions;

namespace GameApis.Models;

public record DomainExceptionResponse(string Error, string Code)
{
    public DomainExceptionResponse(DomainException ex)
        : this(ex.Message, ex.Code.ToString()) { }
}
