using System.Diagnostics;
using System.Net;
using Domain.Common;
using Domain.SeedWork.Notification;
using Infra.Utils.Constants;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace API.Middlewares
{
    public class ControllerMiddleware(
        RequestDelegate next,
        IOptions<JsonOptions> options)
    {
        private static readonly bool IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        private readonly JsonSerializerOptions _jsonOptions = options.Value.JsonSerializerOptions;
        public async Task InvokeAsync(HttpContext context, INotification notification)
        {
            var activity = Activity.Current;
            try
            {
                AppendTraceIdToHeaders(context, activity);
                await next(context);
                activity?.SetStatus(ActivityStatusCode.Ok);
            }
            catch (Exception ex)
            {
                activity?.SetStatus(ActivityStatusCode.Error);
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = ErrorResponse.Factory
                .Create(IsDevelopment ? exception.ToString() : RequestErrorResponseConstant.InternalError);
            UpdateContext(context, HttpStatusCode.InternalServerError);
            var stringResponse = JsonSerializer.Serialize(result, _jsonOptions);
            await context.Response.WriteAsync(stringResponse);
        }
        private static void UpdateContext(HttpContext context, HttpStatusCode code)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            // AppendTraceIdToHeaders(context, Activity.Current);
        }

        private static void AppendTraceIdToHeaders(HttpContext context, Activity? activity)
        {
            var traceId = activity?.TraceId.ToString();
            if (!string.IsNullOrEmpty(traceId) && !context.Response.Headers.ContainsKey("X-Trace-Id"))
                context.Response.Headers.Append("X-Trace-Id", traceId);
        }
    }
}
