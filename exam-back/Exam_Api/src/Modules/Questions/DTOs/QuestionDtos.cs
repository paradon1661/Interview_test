namespace Exam.Modules.Questions.DTOs;

/// <summary>What the exam page shows. No <c>isCorrect</c>: the answer key stays on the server.</summary>
public sealed record QuestionDto(int Id, int No, string QuestionText, IReadOnlyList<QuestionChoiceDto> Choices);

public sealed record QuestionChoiceDto(int Id, string ChoiceText);

public static class QuestionMappings
{
    public static QuestionDto ToDto(this Question q, int no) => new(
        q.Id,
        no,
        q.QuestionText,
        q.Choices.OrderBy(c => c.SortOrder).ThenBy(c => c.Id).Select(c => new QuestionChoiceDto(c.Id, c.ChoiceText)).ToList());
}
