namespace GameApis.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainExceptionCodes Code { get; }

    public DomainException(DomainExceptionCodes code)
        : base($"A business rule was violated: {code}")
    {
        Code = code;
    }
}
