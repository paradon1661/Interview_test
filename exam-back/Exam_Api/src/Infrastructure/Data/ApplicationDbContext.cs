using Microsoft.EntityFrameworkCore;

namespace Exam.Infrastructure.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<QuestionChoice> QuestionChoices => Set<QuestionChoice>();
    public DbSet<ExamResult> ExamResults => Set<ExamResult>();
    public DbSet<ExamAnswer> ExamAnswers => Set<ExamAnswer>();

    /// <summary>Mapping only. The schema itself is created by migrations/*.sql (script/run-migrations.sh) — keep both in step.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
}
