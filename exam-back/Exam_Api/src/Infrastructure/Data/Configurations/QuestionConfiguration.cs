using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exam.Infrastructure.Data.Configurations;

public sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> b)
    {
        b.ToTable("tb_m_question");
        b.HasKey(x => x.Id);
        b.Property(x => x.QuestionText).HasMaxLength(500).IsRequired();
        b.HasMany(x => x.Choices).WithOne().HasForeignKey(c => c.QuestionId);
        b.Navigation(x => x.Choices).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public sealed class QuestionChoiceConfiguration : IEntityTypeConfiguration<QuestionChoice>
{
    public void Configure(EntityTypeBuilder<QuestionChoice> b)
    {
        b.ToTable("tb_m_question_choice");
        b.HasKey(x => x.Id);
        b.Property(x => x.ChoiceText).HasMaxLength(200).IsRequired();
    }
}
