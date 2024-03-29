using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Filter;

[ExcludeFromCodeCoverage]
public class ExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var result = new ObjectResult(new ExceptionModel()
        {
            message = context.Exception.Message,
            detail = context.Exception.InnerException?.Message ?? context.Exception.Message,
        })
        {
            StatusCode = Convert.ToInt32(context.Exception.Data["status_code"])
        };
        context.Result = result;

        Log.Logger = new LoggerConfiguration()
                .CreateLogger();
        Log.Logger.Error("Unhandled exception occurred while executing request: {ex}", context.Exception);

        // Log the exception
        _logger.LogError("Unhandled exception occurred while executing request: {ex}", context.Exception);

        return Task.CompletedTask;
    }
}
