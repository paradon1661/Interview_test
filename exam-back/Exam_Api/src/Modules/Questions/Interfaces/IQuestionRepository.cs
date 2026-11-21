namespace Exam.Modules.Questions.Interfaces;

public interface IQuestionRepository : IBaseRepository<Question>
{
    /// <summary>Active questions with their choices, in exam order. Read-only.</summary>
    Task<IReadOnlyList<Question>> ListActiveAsync(CancellationToken ct);
}
