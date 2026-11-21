namespace Exam.Shared.Middleware;

/// <summary>The current request's correlation id, readable from anywhere (logs, error responses).</summary>
public static class CorrelationContext
{
    public const string HeaderName = "X-Correlation-Id";
    private static readonly AsyncLocal<string?> Current = new();

    public static string? Id
    {
        get => Current.Value;
        set => Current.Value = value;
    }
}
