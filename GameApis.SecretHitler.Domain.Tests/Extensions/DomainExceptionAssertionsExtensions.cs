using FluentAssertions;
using FluentAssertions.Specialized;
using GameApis.Domain.Exceptions;

namespace GameApis.SecretHitler.Domain.Tests.Extensions;

internal static class DomainExceptionAssertionsExtensions
{
    public static ExceptionAssertions<DomainException> WithCode(this ExceptionAssertions<DomainException> assertions, DomainExceptionCodes expectedCode, string? because = null)
    {
        assertions.Which.Code.Should().Be(expectedCode, because);
        return assertions;
    }
}
