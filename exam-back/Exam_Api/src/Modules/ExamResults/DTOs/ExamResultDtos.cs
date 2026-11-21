namespace Exam.Modules.ExamResults.DTOs;

public sealed record SubmitExamRequest(string FullName, IReadOnlyList<SubmitAnswerItem> Answers);

public sealed record SubmitAnswerItem(int QuestionId, int ChoiceId);

/// <summary>หน้า IT 10-2: ชื่อ คะแนน และคำตอบที่เลือก (ไม่บอกว่าข้อไหนถูก เพื่อไม่ให้เฉลยรั่ว).</summary>
public sealed record ExamResultDto(
    Guid Id,
    string FullName,
    int Score,
    int TotalQuestions,
    DateTimeOffset CreateAt,
    IReadOnlyList<ExamAnswerDto> Answers);

public sealed record ExamAnswerDto(int QuestionId, int ChoiceId);

public static class ExamResultMappings
{
    public static ExamResultDto ToDto(this ExamResult r) => new(
        r.Id, r.FullName, r.Score, r.TotalQuestions, r.CreateAt,
        r.Answers.Select(a => new ExamAnswerDto(a.QuestionId, a.ChoiceId)).ToList());

    public static IReadOnlyList<SelectedAnswer> ToSelectedAnswers(this SubmitExamRequest r) =>
        (r.Answers ?? []).Select(a => new SelectedAnswer(a.QuestionId, a.ChoiceId)).ToList();
}
