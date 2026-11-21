using FluentValidation;

namespace Exam.Modules.ExamResults.Validators;

public sealed class SubmitExamRequestValidator : AbstractValidator<SubmitExamRequest>
{
    public SubmitExamRequestValidator()
    {
        RuleFor(x => x.FullName)
            .Must(n => !string.IsNullOrWhiteSpace(n)).WithMessage("กรุณากรอกชื่อ-สกุล")
            .MaximumLength(200).WithMessage("ชื่อ-สกุลต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(x => x.Answers).NotNull().WithMessage("ไม่พบคำตอบ");
        RuleForEach(x => x.Answers).ChildRules(a =>
        {
            a.RuleFor(x => x.QuestionId).GreaterThan(0);
            a.RuleFor(x => x.ChoiceId).GreaterThan(0);
        });
        RuleFor(x => x.Answers)
            .Must(list => list.Select(a => a.QuestionId).Distinct().Count() == list.Count)
            .When(x => x.Answers is not null)
            .WithMessage("แต่ละข้อเลือกคำตอบได้เพียงข้อเดียว");
    }
}
