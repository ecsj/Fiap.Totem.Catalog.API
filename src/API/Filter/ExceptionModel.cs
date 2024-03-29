using System.Diagnostics.CodeAnalysis;

namespace API.Filter;

[ExcludeFromCodeCoverage]

public struct ExceptionModel
{
    public string message { get; set; }
    public string detail { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
}
