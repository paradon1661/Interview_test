using Microsoft.EntityFrameworkCore;

namespace Exam.Modules.Questions.Repositories;

public sealed class QuestionRepository(ApplicationDbContext db) : BaseRepository<Question>(db), IQuestionRepository
{
    public async Task<IReadOnlyList<Question>> ListActiveAsync(CancellationToken ct) =>
        await Db.Questions.AsNoTracking()
            .Include(q => q.Choices)
            .Where(q => q.IsActive)
            .OrderBy(q => q.SortOrder).ThenBy(q => q.Id)
            .ToListAsync(ct);
}
