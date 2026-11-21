using Microsoft.EntityFrameworkCore;

namespace Exam.Modules.ExamResults.Repositories;

public sealed class ExamResultRepository(ApplicationDbContext db) : BaseRepository<ExamResult>(db), IExamResultRepository
{
    public Task<ExamResult?> GetAsync(Guid id, CancellationToken ct) =>
        Db.ExamResults.AsNoTracking().Include(r => r.Answers).SingleOrDefaultAsync(r => r.Id == id, ct);
}
