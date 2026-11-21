namespace Exam.Modules.ExamResults.Entities;

/// <summary>คำตอบที่ผู้สอบเลือก ก่อนตรวจ.</summary>
public sealed record SelectedAnswer(int QuestionId, int ChoiceId);

/// <summary>
/// ผลสอบ 1 ครั้ง. The grading rule lives here (<see cref="Grade"/>), so it is unit tested without a database
/// and the score can only ever be computed on the server.
/// </summary>
public sealed class ExamResult
{
    private readonly List<ExamAnswer> _answers = [];

    private ExamResult() { }

    public Guid Id { get; private set; }
    public string FullName { get; private set; } = "";
    public int Score { get; private set; }
    public int TotalQuestions { get; private set; }
    public DateTimeOffset CreateAt { get; private set; }
    public IReadOnlyList<ExamAnswer> Answers => _answers;

    /// <summary>
    /// ตรวจข้อสอบ: 1 ข้อ ตอบได้ 1 คำตอบ, ตัวเลือกต้องเป็นของข้อนั้น, ข้อที่ไม่ได้ตอบ = ผิด.
    /// คะแนนเต็ม = จำนวนข้อสอบทั้งหมด (ไม่ใช่จำนวนข้อที่ตอบ).
    /// </summary>
    public static ExamResult Grade(string fullName, IReadOnlyCollection<Question> questions, IReadOnlyCollection<SelectedAnswer> answers, DateTimeOffset now)
    {
        var name = fullName?.Trim() ?? "";
        if (name.Length == 0) throw new DomainException("full_name_required", "กรุณากรอกชื่อ-สกุล");

        var duplicate = answers.GroupBy(a => a.QuestionId).FirstOrDefault(g => g.Count() > 1);
        if (duplicate is not null)
            throw new DomainException("duplicate_answer", "แต่ละข้อเลือกคำตอบได้เพียงข้อเดียว");

        var questionById = questions.ToDictionary(q => q.Id);
        var result = new ExamResult
        {
            Id = Guid.CreateVersion7(),
            FullName = name,
            TotalQuestions = questions.Count,
            CreateAt = now,
        };

        foreach (var answer in answers)
        {
            var choice = questionById.GetValueOrDefault(answer.QuestionId)?.FindChoice(answer.ChoiceId)
                ?? throw new DomainException("invalid_answer", "คำตอบไม่ตรงกับข้อสอบ กรุณาโหลดหน้าใหม่แล้วลองอีกครั้ง");
            result._answers.Add(new ExamAnswer(Guid.CreateVersion7(), result.Id, answer.QuestionId, choice.Id, choice.IsCorrect));
        }

        result.Score = result._answers.Count(a => a.IsCorrect);
        return result;
    }
}
