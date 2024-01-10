using System.Diagnostics.CodeAnalysis;

namespace Domain;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    { }

    public static void ThrowIfEmpty(string? value, string resourceName)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new DomainException($"{resourceName}を空にすることはできません。");
    }

    public static void ThrowIfNotFound<T>([NotNull] T? target, string resourceName)
    {
        if (target is null) throw new DomainException($"対象の{resourceName}が存在しません。");
    }
}