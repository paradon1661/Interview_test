namespace Exam.Modules.ExamResults.Entities;

public sealed class ExamAnswer
{
    private ExamAnswer() { }

    internal ExamAnswer(Guid id, Guid examResultId, int questionId, int choiceId, bool isCorrect)
    {
        Id = id;
        ExamResultId = examResultId;
        QuestionId = questionId;
        ChoiceId = choiceId;
        IsCorrect = isCorrect;
    }

    public Guid Id { get; private set; }
    public Guid ExamResultId { get; private set; }
    public int QuestionId { get; private set; }
    public int ChoiceId { get; private set; }
    /// <summary>Snapshot at grading time: a later change to the answer key does not rewrite old results.</summary>
    public bool IsCorrect { get; private set; }
}
