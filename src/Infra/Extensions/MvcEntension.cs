using Infra.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Infra.Extensions
{
    public static class MvcEntension
    {
        public static IServiceCollection AddConfiguredMvc(this IServiceCollection services)
        {
            //services.AddMvcCore()
            //   .AddJsonOptions(options =>
            //   {
            //       options.SerializerSettings.DateFormatString = "dd/MM/yyyy";
            //       //Formata saída das APIs para camelCase.
            //       options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //       // Remove valores nulos das respostas
            //       options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //   });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var listaDeCamposComProblema = context.ModelState.Where(a => a.Value.Errors.Any()).Select(x => x.Key);
                    var camposComProblema = string.Join(", ", listaDeCamposComProblema);

                    var responseErro = new ResponseErro
                    {
                        Status = "400",
                        Mensagem = camposComProblema != string.Empty ?
                                   $"Um ou mais campos estão inválidos. Refaça sua requisição corrigindo o(s) seguinte(s) elemento(s): { camposComProblema }." : "BadRequest"
                    };

                    var result = new BadRequestObjectResult(responseErro);

                    result.ContentTypes.Add("application/problem+json");
                    result.ContentTypes.Add("application/problem+xml");

                    return result;
                };
            });

            return services;
        }

        public static IApplicationBuilder UseError(this IApplicationBuilder builder) =>
                  builder.UseMiddleware<ErrorMiddleware>();

        internal class ResponseErro
        {
            public string Status { get; set; }
            public string Mensagem { get; set; }
        }
    }
}
