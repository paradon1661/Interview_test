using Exam.Modules.ExamResults.Entities;
using Exam.Modules.Questions.Entities;
using Exam.Shared.Exceptions;

namespace Exam.Tests;

/// <summary>ExamResult.Grade — the scoring rule, without a database.</summary>
public class ExamGradingTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 20, 9, 0, 0, TimeSpan.Zero);

    // Same as migrations/003_seed_questions.sql: Q1 → 9 (choice 3), Q2 → 2 (choice 6)
    private static readonly Question[] Questions =
    [
        new(1, "ข้อใดต่างจากข้ออื่น", 1, [new(1, "3", false, 1), new(2, "5", false, 2), new(3, "9", true, 3), new(4, "11", false, 4)]),
        new(2, "X + 2 = 4 จงหาค่า X", 2, [new(5, "1", false, 1), new(6, "2", true, 2), new(7, "3", false, 3), new(8, "4", false, 4)]),
    ];

    [Fact]
    public void All_correct_scores_full_marks()
    {
        var r = ExamResult.Grade("สมศรี เกิดผล", Questions, [new(1, 3), new(2, 6)], Now);

        Assert.Equal(2, r.Score);
        Assert.Equal(2, r.TotalQuestions);
        Assert.All(r.Answers, a => Assert.True(a.IsCorrect));
    }

    [Fact]
    public void One_wrong_scores_one_of_two()
    {
        var r = ExamResult.Grade("สมศรี เกิดผล", Questions, [new(1, 1), new(2, 6)], Now);

        Assert.Equal(1, r.Score);
        Assert.Equal(2, r.TotalQuestions);
    }

    [Fact]
    public void Unanswered_question_counts_as_wrong_but_stays_in_the_total()
    {
        var r = ExamResult.Grade("สมศรี เกิดผล", Questions, [new(2, 6)], Now);

        Assert.Equal(1, r.Score);
        Assert.Equal(2, r.TotalQuestions);
        Assert.Single(r.Answers);
    }

    [Fact]
    public void Name_is_trimmed_and_result_stamped()
    {
        var r = ExamResult.Grade("  สมศรี เกิดผล  ", Questions, [], Now);

        Assert.Equal("สมศรี เกิดผล", r.FullName);
        Assert.Equal(Now, r.CreateAt);
        Assert.NotEqual(Guid.Empty, r.Id);
        Assert.All(r.Answers, a => Assert.Equal(r.Id, a.ExamResultId));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Blank_name_is_rejected(string name)
    {
        var ex = Assert.Throws<DomainException>(() => ExamResult.Grade(name, Questions, [], Now));
        Assert.Equal("full_name_required", ex.Code);
    }

    [Fact]
    public void Two_answers_for_one_question_are_rejected()
    {
        var ex = Assert.Throws<DomainException>(() => ExamResult.Grade("A", Questions, [new(1, 3), new(1, 4)], Now));
        Assert.Equal("duplicate_answer", ex.Code);
    }

    [Theory]
    [InlineData(1, 6)]   // choice of another question
    [InlineData(1, 99)]  // unknown choice
    [InlineData(99, 3)]  // unknown question
    public void Choice_must_belong_to_its_question(int questionId, int choiceId)
    {
        var ex = Assert.Throws<DomainException>(() => ExamResult.Grade("A", Questions, [new(questionId, choiceId)], Now));
        Assert.Equal("invalid_answer", ex.Code);
    }
}
