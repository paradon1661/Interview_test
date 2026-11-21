using FluentValidation;

namespace Exam.Shared.Middleware;

/// <summary>Turns every exception into an HTTP status + <c>Result.Fail(code, message)</c>.</summary>
public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> log)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // client went away; nothing to answer
        }
        catch (Exception ex)
        {
            var (status, code, message, details) = Map(ex);
            if (status >= 500) log.LogError(ex, "Unhandled {Code}", code);
            else log.LogInformation("Request failed {Status} {Code}: {Message}", status, code, message);

            if (context.Response.HasStarted) throw;
            context.Response.Clear();
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(Result.Fail(code, message, details));
        }
    }

    private static (int Status, string Code, string Message, IDictionary<string, string[]>? Details) Map(Exception ex) => ex switch
    {
        ValidationException v => (400, "validation_failed", v.Errors.FirstOrDefault()?.ErrorMessage ?? "ข้อมูลไม่ถูกต้อง",
            v.Errors.GroupBy(e => e.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())),
        DomainException d => (422, d.Code, d.Message, null),
        NotFoundException n => (404, n.Code, n.Message, null),
        _ => (500, "internal_error", "เกิดข้อผิดพลาดในระบบ กรุณาลองใหม่อีกครั้ง", null),
    };
}
