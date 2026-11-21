using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exam.Infrastructure.Data.Configurations;

public sealed class ExamResultConfiguration : IEntityTypeConfiguration<ExamResult>
{
    public void Configure(EntityTypeBuilder<ExamResult> b)
    {
        b.ToTable("tb_exam_result");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        b.HasMany(x => x.Answers).WithOne().HasForeignKey(a => a.ExamResultId);
        b.Navigation(x => x.Answers).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public sealed class ExamAnswerConfiguration : IEntityTypeConfiguration<ExamAnswer>
{
    public void Configure(EntityTypeBuilder<ExamAnswer> b)
    {
        b.ToTable("tb_exam_answer");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();
    }
}
