using System.Net;
using Infra.Utils.Constants;
using Newtonsoft.Json;

namespace API.Middlewares
{
    public class ControllerMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = JsonConvert.SerializeObject(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ?
                new { error = exception.Message } :
                new { error = RequestErrorResponseConstant.InternalError });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}
