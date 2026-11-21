namespace Exam.Shared.Middleware;

/// <summary>Takes the caller's X-Correlation-Id (or makes one), echoes it back and puts it on every log line.</summary>
public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> log)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var incoming = context.Request.Headers[CorrelationContext.HeaderName].ToString();
        var id = !string.IsNullOrWhiteSpace(incoming) && incoming.Length <= 64 ? incoming : Guid.NewGuid().ToString("N");

        CorrelationContext.Id = id;
        context.Response.Headers[CorrelationContext.HeaderName] = id;
        using (log.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = id }))
            await next(context);
    }
}
