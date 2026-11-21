namespace Exam.Shared.Services;

/// <summary>"Now", injectable so rules that depend on time can be tested.</summary>
public interface IClock
{
    DateTimeOffset UtcNow { get; }
}

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
