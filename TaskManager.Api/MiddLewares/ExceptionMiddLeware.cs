
using System.Net;

namespace TaskManager.Api.MiddLewares
{
    public class ExceptionMiddLeware
    {        
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddLeware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddLeware(RequestDelegate next, ILogger<ExceptionMiddLeware> logger, IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ERROR] {ex.Message}");

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


                object response;
                if (_environment.IsDevelopment())
                {
                    response = (new { error = ex.Message, StackTrace = ex.StackTrace });
                }
                else
                {
                    response = (new { error = "An internal server error ocurred" });
                }

                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
