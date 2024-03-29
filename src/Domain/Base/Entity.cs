using System.Diagnostics.CodeAnalysis;

namespace Domain.Base;

[ExcludeFromCodeCoverage]

public abstract class Entity
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now.BrazilDateTime();
}