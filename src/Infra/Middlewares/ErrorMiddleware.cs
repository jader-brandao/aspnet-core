using Infra.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var status = exception.GetHttpStatusCode();
            var mensagem = status == HttpStatusCode.InternalServerError ? "Internal error." : exception.Message;
                      

            var result = JsonConvert.SerializeObject(new ErrorDto(status, mensagem), new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }
    }

    public class ErrorDto
    {
        public string Status { get; set; }
        public string Mensagem { get; set; }

        public ErrorDto() { }

        public ErrorDto(HttpStatusCode status, string mensagem)
        {
            Status = $"{Convert.ToInt32(status)}";
            Mensagem = mensagem;
        }
    }
}
