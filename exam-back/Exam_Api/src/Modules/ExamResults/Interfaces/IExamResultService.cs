namespace Exam.Modules.ExamResults.Interfaces;

public interface IExamResultService
{
    /// <summary>ตรวจคำตอบ คิดคะแนน และบันทึกผลสอบ (1 transaction).</summary>
    Task<ExamResultDto> SubmitAsync(SubmitExamRequest request, CancellationToken ct);

    Task<ExamResultDto> GetAsync(Guid id, CancellationToken ct);
}
