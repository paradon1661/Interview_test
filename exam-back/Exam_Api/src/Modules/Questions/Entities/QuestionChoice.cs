namespace Exam.Modules.Questions.Entities;

public sealed class QuestionChoice
{
    private QuestionChoice() { }

    public QuestionChoice(int id, string choiceText, bool isCorrect, int sortOrder)
    {
        Id = id;
        ChoiceText = choiceText;
        IsCorrect = isCorrect;
        SortOrder = sortOrder;
    }

    public int Id { get; private set; }
    public int QuestionId { get; private set; }
    public string ChoiceText { get; private set; } = "";
    /// <summary>เฉลย. Never sent to the browser.</summary>
    public bool IsCorrect { get; private set; }
    public int SortOrder { get; private set; }
}
