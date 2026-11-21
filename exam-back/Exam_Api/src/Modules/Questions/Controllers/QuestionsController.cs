using Microsoft.AspNetCore.Mvc;

namespace Exam.Modules.Questions.Controllers;

[ApiController]
public sealed class QuestionsController(IQuestionService questions) : ControllerBase
{
    /// <summary>ข้อสอบทั้งหมดสำหรับหน้า IT 10-1 (ไม่มีเฉลย).</summary>
    [HttpGet("/api/questions")]
    public async Task<Result<IReadOnlyList<QuestionDto>>> List(CancellationToken ct) =>
        Result.Ok(await questions.ListAsync(ct));
}
