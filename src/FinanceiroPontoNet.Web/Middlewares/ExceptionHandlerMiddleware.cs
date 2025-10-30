using System.Net;
using System.Text.Json;
using FinanceiroPontoNet.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FinanceiroPontoNet.Web.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            IHostEnvironment env,
            ILogger<ExceptionHandlerMiddleware> logger
        )
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: {Message}", ex.Message);
                httpContext.Response.ContentType = "application/json";
                var responseBody = new
                {
                    message = "Ocorreu um erro interno no servidor.",
                    details = "Erro interno.",
                };
                switch (ex)
                {
                    case ArgumentException ae:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseBody = new { message = ae.Message, details = "Dados incorretos." };
                        break;
                    case NotFoundException nf:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseBody = new { message = nf.Message, details = "Busca incorreta." };
                        break;
                    default:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        if (_env.IsDevelopment())
                        {
                            responseBody = new
                            {
                                message = ex.Message,
                                details = ex.StackTrace ?? "StackTrace vazia.",
                            };
                        }
                        break;
                }

                var jsonResponse = JsonSerializer.Serialize(responseBody);
                await httpContext.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
