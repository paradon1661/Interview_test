namespace Exam.Modules.Questions.Interfaces;

public interface IQuestionService
{
    Task<IReadOnlyList<QuestionDto>> ListAsync(CancellationToken ct);
}
