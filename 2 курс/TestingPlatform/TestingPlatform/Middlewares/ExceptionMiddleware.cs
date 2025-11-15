using TestingPlatform.Infrastructure.Exceptions;

namespace practice.Middlewares;

/// <summary>
/// Промежуточное ПО перехвата исключений.
/// </summary>
public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception exception)
        {
            var errorCodeResponse = exception switch
            {
                InvalidOperationException exc => new CodeAndMessage(StatusCodes.Status400BadRequest, exc.Message),
                ArgumentException exc => new CodeAndMessage(StatusCodes.Status400BadRequest, exc.Message),
                EntityNotFoundException exc => new CodeAndMessage(StatusCodes.Status404NotFound, exc.Message),
                _ => new CodeAndMessage(StatusCodes.Status500InternalServerError, "Internal server error")
            };
            await HandleExceptionAsync(httpContext, errorCodeResponse);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, CodeAndMessage errorCodeResponse)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorCodeResponse.HttpStatusCode;

        await context.Response.WriteAsJsonAsync(new { message = errorCodeResponse.Message });
    }

    private record struct CodeAndMessage(int HttpStatusCode, string Message);
}
