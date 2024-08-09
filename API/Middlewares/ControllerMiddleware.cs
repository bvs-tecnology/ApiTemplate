using System.Net;
using Domain.Common;
using Domain.Exceptions;
using Domain.SeedWork.Notification;
using Infra.Utils.Constants;
using Newtonsoft.Json;
using static System.String;

namespace API.Middlewares
{
    public class ControllerMiddleware(RequestDelegate next, ILogger<ControllerMiddleware> logger)
    {
        private static readonly bool IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        public async Task Invoke(HttpContext context, INotification notification)
        {
            try
            {
                await next(context);
                LogInformation(context);
            }
            catch (NotificationException)
            {
                await HandleNotificationExceptionAsync(context, notification.Notifications);
            }
            catch (NotAllowedException)
            {
                await HandleNotAllowedException(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task<string> GetBody(HttpContext context)
        {
            context.Request?.EnableBuffering();
            if (context.Request?.Body is null) return Empty;
            var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            return body;
        }

        private void LogInformation(HttpContext context)
        {
            logger.LogInformation("Request {method} {url} returned HttpStatusCode {statusCode}",
                context.Request?.Method,
                context.Request?.Path.Value,
                context.Response?.StatusCode);
        }

        private void LogError(HttpContext context)
        {
            logger.LogError("Request {method} {url} returned HttpStatusCode {statusCode}",
                context.Request?.Method,
                context.Request?.Path.Value,
                context.Response?.StatusCode);
        }

        private Task HandleNotificationExceptionAsync(HttpContext context, List<NotificationModel> notifications)
        {
            var result = new GenericResponse<object>();
            notifications.ForEach(x => result.AddError(x.Message));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            LogError(context);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = new GenericResponse<object>();
            result.AddError(IsDevelopment ? exception.Message : RequestErrorResponseConstant.InternalError);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            LogError(context);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }

        private Task HandleNotAllowedException(HttpContext context)
        {
            var result = new GenericResponse<object>();
            result.AddError(RequestErrorResponseConstant.NotAllowed);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;

            LogError(context);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
