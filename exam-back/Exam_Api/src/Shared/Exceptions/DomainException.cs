namespace Exam.Shared.Exceptions;

/// <summary>A business rule was broken. <see cref="Code"/> is the stable machine-readable code the FE receives.</summary>
public class DomainException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}
