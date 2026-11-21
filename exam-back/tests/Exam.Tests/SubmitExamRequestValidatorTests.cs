using Exam.Modules.ExamResults.DTOs;
using Exam.Modules.ExamResults.Validators;

namespace Exam.Tests;

public class SubmitExamRequestValidatorTests
{
    private readonly SubmitExamRequestValidator _validator = new();

    [Fact]
    public void Valid_request_passes() =>
        Assert.True(_validator.Validate(new SubmitExamRequest("สมศรี เกิดผล", [new(1, 3), new(2, 6)])).IsValid);

    [Fact]
    public void Blank_name_fails() =>
        Assert.Contains(_validator.Validate(new SubmitExamRequest(" ", [])).Errors, e => e.PropertyName == "FullName");

    [Fact]
    public void Name_over_200_chars_fails() =>
        Assert.False(_validator.Validate(new SubmitExamRequest(new string('ก', 201), [])).IsValid);

    [Fact]
    public void Duplicate_question_fails() =>
        Assert.False(_validator.Validate(new SubmitExamRequest("A", [new(1, 3), new(1, 4)])).IsValid);
}
