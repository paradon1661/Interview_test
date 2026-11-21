namespace Exam.Modules.Questions.Entities;

/// <summary>ข้อสอบ 1 ข้อ (master data). ตัวเลือกที่ถูกมีได้ตัวเดียว — บังคับด้วย unique index ใน migration 001.</summary>
public sealed class Question
{
    private readonly List<QuestionChoice> _choices = [];

    private Question() { }

    public Question(int id, string questionText, int sortOrder, IEnumerable<QuestionChoice> choices)
    {
        Id = id;
        QuestionText = questionText;
        SortOrder = sortOrder;
        IsActive = true;
        _choices.AddRange(choices);
    }

    public int Id { get; private set; }
    public string QuestionText { get; private set; } = "";
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreateAt { get; private set; }
    public IReadOnlyList<QuestionChoice> Choices => _choices;

    public QuestionChoice? FindChoice(int choiceId) => _choices.FirstOrDefault(c => c.Id == choiceId);
}
