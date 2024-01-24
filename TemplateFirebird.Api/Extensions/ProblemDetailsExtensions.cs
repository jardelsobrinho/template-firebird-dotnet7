using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TemplateFirebird.Api.Extensions;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddProblemDetailsModelState(this IServiceCollection services)
    {
        return services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                KeyValuePair<string, ModelStateEntry?> error = context.ModelState.FirstOrDefault();

                string title = "";
                string detalhes = "";
                foreach (KeyValuePair<string, ModelStateEntry> e in context.ModelState)
                {
                    if (e.Value.Errors.Count > 0)
                    {
                        title = e.Value.Errors[0].ErrorMessage;
                    }

                    int indice = 0;
                    if (e.Value.Errors.Count > 1)
                    {
                        indice++;
                        detalhes += e.Value.Errors[indice].ErrorMessage;
                    }
                }

                ValidationProblemDetails problemDetails = new(context.ModelState)
                {
                    Title = title,
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = detalhes,
                };

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json", "application/problem+xml" }
                };
            };
        });
    }
}
