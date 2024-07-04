// using System.Text.Json;
using System.Net;
using Application.Helper;
using Application.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
    {
        object errors = null;
        var message = string.Empty;
        switch (ex)
        {
            case RestException re:
                logger.LogError($"{ex.Message}: \n {ex.StackTrace}");
                message = re.ErrorMessage;
                errors = re.Errors;
                context.Response.StatusCode = (int)re.Code;
                break;
            case Exception e:
                logger.LogError($"{ex.Message}: \n {ex.StackTrace}");
                message = e.Message;
                errors = "Server Error";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }
        DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        var response = new ErrorResponse<object>
        {
            Message = message,
            Error = errors
        };

        context.Response.ContentType = "application/json";

        var result = JsonConvert.SerializeObject(response, new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Newtonsoft.Json.Formatting.Indented
        });
        await context.Response.WriteAsync(result);

    }
}
// Extension method used to add the middleware to the HTTP request pipeline.
public static class ErrorHandlingMiddlewareExtension
{
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}

