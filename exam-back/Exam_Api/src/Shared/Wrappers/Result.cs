namespace Exam.Shared.Wrappers;

/// <summary>
/// Every JSON response is wrapped: <c>{ "success": true, "data": … }</c> or <c>{ "success": false, "error": { code, message } }</c>.
/// The HTTP status code still carries the meaning (200 / 400 / 404 / 422 / 500) — the wrapper never replaces it.
/// exam-front's apiFetch.js unwraps <c>data</c> and reads <c>error.code</c>.
/// </summary>
public sealed record Result<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public ResultError? Error { get; init; }
}

public sealed record ResultError(string Code, string Message, string? CorrelationId = null, IDictionary<string, string[]>? Details = null);

public static class Result
{
    public static Result<T> Ok<T>(T data) => new() { Success = true, Data = data };

    public static Result<object?> Fail(string code, string message, IDictionary<string, string[]>? details = null) =>
        new() { Success = false, Error = new ResultError(code, message, CorrelationContext.Id, details) };
}
