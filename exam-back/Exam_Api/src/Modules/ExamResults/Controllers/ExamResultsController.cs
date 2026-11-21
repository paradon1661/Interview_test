using Microsoft.AspNetCore.Mvc;

namespace Exam.Modules.ExamResults.Controllers;

[ApiController]
public sealed class ExamResultsController(IExamResultService examResults) : ControllerBase
{
    /// <summary>ส่งข้อสอบ: ตรวจ คิดคะแนน บันทึก แล้วคืนผลสำหรับหน้า IT 10-2.</summary>
    [HttpPost("/api/exam-results")]
    public async Task<Result<ExamResultDto>> Submit(SubmitExamRequest request, CancellationToken ct) =>
        Result.Ok(await examResults.SubmitAsync(request, ct));

    /// <summary>ผลสอบที่บันทึกไว้ (หน้า IT 10-2 เปิดซ้ำ / refresh ได้).</summary>
    [HttpGet("/api/exam-results/{id:guid}")]
    public async Task<Result<ExamResultDto>> Get(Guid id, CancellationToken ct) =>
        Result.Ok(await examResults.GetAsync(id, ct));
}
