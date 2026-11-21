namespace Exam.Modules.Questions.Services;

public sealed class QuestionService(IQuestionRepository questions) : IQuestionService
{
    public async Task<IReadOnlyList<QuestionDto>> ListAsync(CancellationToken ct) =>
        (await questions.ListActiveAsync(ct)).Select((q, i) => q.ToDto(i + 1)).ToList();
}
