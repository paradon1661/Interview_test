namespace Exam.Modules.ExamResults.Interfaces;

public interface IExamResultRepository : IBaseRepository<ExamResult>
{
    Task<ExamResult?> GetAsync(Guid id, CancellationToken ct);
}
