using System.Net;
using Domain.Common;
using Domain.SeedWork.Notification;
using Infra.Utils.Constants;
using System.Text.Json;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Middlewares
{
    public class ExceptionMiddleware(
        RequestDelegate next,
        IOptions<JsonOptions> options,
        IHostEnvironment environment
    )
    {
        private readonly JsonSerializerOptions _jsonOptions = options.Value.JsonSerializerOptions;
        public async Task InvokeAsync(HttpContext context, INotification notification)
        {
            try
            {
                await next(context);
            }
            catch (NotificationException)
            {
                await HandleExceptionAsync(context, notification);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, INotification notification)
        {
            var result = ErrorResponse.Factory.Create(notification.Notifications);
            UpdateContext(context, HttpStatusCode.InternalServerError);
            var stringResponse = JsonSerializer.Serialize(result, _jsonOptions);
            await context.Response.WriteAsync(stringResponse);
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = ErrorResponse.Factory.Create(
                environment.IsDevelopment() 
                ? exception.ToString() 
                : RequestErrorResponseConstant.InternalError
            );
            UpdateContext(context, HttpStatusCode.InternalServerError);
            var stringResponse = JsonSerializer.Serialize(result, _jsonOptions);
            await context.Response.WriteAsync(stringResponse);
        }
        private static void UpdateContext(HttpContext context, HttpStatusCode code)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
        }
    }
}
