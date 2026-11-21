namespace Exam.Modules.ExamResults.Services;

public sealed class ExamResultService(
    IExamResultRepository results,
    IQuestionRepository questions,
    IClock clock,
    ILogger<ExamResultService> log) : IExamResultService
{
    public async Task<ExamResultDto> SubmitAsync(SubmitExamRequest request, CancellationToken ct)
    {
        var activeQuestions = await questions.ListActiveAsync(ct);
        var result = ExamResult.Grade(request.FullName, activeQuestions, request.ToSelectedAnswers(), clock.UtcNow);

        results.Add(result);
        await results.SaveChangesAsync(ct);

        log.LogInformation("Exam submitted {ResultId}: {Score}/{Total}", result.Id, result.Score, result.TotalQuestions);
        return result.ToDto();
    }

    public async Task<ExamResultDto> GetAsync(Guid id, CancellationToken ct) =>
        (await results.GetAsync(id, ct) ?? throw new NotFoundException("ผลสอบ")).ToDto();
}
