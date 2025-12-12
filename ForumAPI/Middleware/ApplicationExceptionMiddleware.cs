using ForumApi.Exeptions;

namespace ForumApi.Middlewares
{
    public class ApplicationExceptionMiddleware
    {
        public readonly RequestDelegate _next;
        public ApplicationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleException(context, exception);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            int statusCode;
            Object result = string.Empty;

            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    result = notFoundException.Message;
                    break;

                case BadRequestException badRequestException:
                    statusCode = StatusCodes.Status400BadRequest;
                    result = badRequestException.Message;
                    break;
                case ForbiddenException unauthorizedException:
                    statusCode = StatusCodes.Status403Forbidden;
                    result = unauthorizedException.Message;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    result = exception.Message;
                    break;
            }

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(result.ToString());
        }
    }
}