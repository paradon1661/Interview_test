using System.Globalization;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Server-side formatting (logs, numbers) must not follow the host's locale — a th-TH box would log year 2569.
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

#region Database
// The schema comes from migrations/*.sql; EF only maps.
services.AddDbContext<ApplicationDbContext>(o => o
    .UseNpgsql(config.GetConnectionString("Exam") ?? throw new InvalidOperationException("ConnectionStrings:Exam is not configured."))
    .UseSnakeCaseNamingConvention());
services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>("postgres", tags: ["ready"]);
#endregion

#region Shared services
services.AddSingleton<IClock, SystemClock>();
#endregion

#region Modules
services.AddScoped<IQuestionRepository, QuestionRepository>();
services.AddScoped<IQuestionService, QuestionService>();
services.AddScoped<IExamResultRepository, ExamResultRepository>();
services.AddScoped<IExamResultService, ExamResultService>();
#endregion

#region API: CORS, controllers
services.AddValidatorsFromAssemblyContaining<SubmitExamRequestValidator>(includeInternalTypes: true);

// Same-origin in practice (Vite proxy), so CORS is closed unless origins are listed explicitly.
var origins = (config.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? []).Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();
services.AddCors(o => o.AddDefaultPolicy(p =>
{
    if (origins.Length > 0)
        p.WithOrigins(origins).AllowAnyMethod().WithHeaders("Content-Type", CorrelationContext.HeaderName);
}));

services.AddControllers(o => o.Filters.Add<ValidationFilter>())
    .ConfigureApiBehaviorOptions(o => o.InvalidModelStateResponseFactory = ctx => new BadRequestObjectResult(Result.Fail(
        "validation_failed", "ข้อมูลไม่ถูกต้อง",
        ctx.ModelState.Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(e => e.Key, e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray()))));
services.AddOpenApi();
#endregion

var app = builder.Build();

#region Middleware (order matters)
app.UseMiddleware<CorrelationIdMiddleware>();     // 1. id available to everything below
app.UseMiddleware<ExceptionHandlingMiddleware>(); // 2. exceptions → Result.Fail
if (app.Environment.IsDevelopment())
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "Exam API")); // /swagger
app.UseCors();
app.MapControllers();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
#endregion

#region Health checks
// Live = process is up; ready = can reach Postgres.
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = c => c.Tags.Contains("ready") });
#endregion

app.Run();

public partial class Program;
