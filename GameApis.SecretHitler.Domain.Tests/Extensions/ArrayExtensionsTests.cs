using FluentAssertions;
using GameApis.SecretHitler.Domain.Extensions;
using Xunit;

namespace GameApis.SecretHitler.Domain.Tests.Extensions;

public class ArrayExtensionsTests
{
    [Fact]
    public void SwapIndices_CorrectlySwapsIndices()
    {
        // arrange
        var sourceArray = new int[] { 1, 2, 3, 4, 5 };

        // act
        sourceArray.SwapIndices(1, 2);

        // assert
        sourceArray.Should().BeEquivalentTo(new[] { 1, 3, 2, 4, 5 });
    }

    [Fact]
    public void SwapIndices_IgnoresSwapWhenIndicesAreEquivalent()
    {
        // arrange
        var sourceArray = new int[] { 1, 2, 3, 4, 5 };

        // act
        sourceArray.SwapIndices(1, 1);

        // assert
        sourceArray.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
    }
}
