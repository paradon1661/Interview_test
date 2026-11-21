namespace Exam.Shared.Exceptions;

/// <summary>Mapped to HTTP by ExceptionHandlingMiddleware. <c>Code</c> ends up in <c>{ error: { code } }</c>.</summary>
public abstract class AppException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}

public sealed class NotFoundException(string what) : AppException("not_found", $"ไม่พบ{what}");
