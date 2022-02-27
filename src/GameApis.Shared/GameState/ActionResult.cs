using System.Diagnostics.CodeAnalysis;

namespace GameApis.Shared.GameState;

public class ActionResult
{
    [MemberNotNullWhen(false, nameof(FailReason))]
    public bool IsSuccess { get; init; }
    public string? FailReason { get; init; } = string.Empty;

    private ActionResult(string? failReason = null)
    {
        IsSuccess = failReason is null;
        FailReason = failReason;
    }

    public static ActionResult Success()
    {
        return new ActionResult();
    }

    public static ActionResult Fail(string reason)
    {
        return new ActionResult(reason);
    }

    public static implicit operator Task<ActionResult>(ActionResult result)
    {
        return Task.FromResult(result);
    }
}