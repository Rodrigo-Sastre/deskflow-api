using System.Net;
using System.Text.Json;

namespace DeskFlow.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Deixa a requisição seguir o fluxo normal (Controller, Service, etc.)
                await _next(context);
            }
            catch (Exception ex)
            {
                // Se algum erro não tratado estourar em qualquer lugar, cai aqui
                await TratarExcecaoAsync(context, ex);
            }
        }

        private static Task TratarExcecaoAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var resposta = new
            {
                sucesso = false,
                mensagem = "Ocorreu um erro interno no servidor. Por favor, tente novamente mais tarde.",
                detalhe = exception.Message
            };

            var json = JsonSerializer.Serialize(resposta);
            return context.Response.WriteAsync(json);
        }
    }
}