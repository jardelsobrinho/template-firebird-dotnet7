using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TemplateFirebird.Api.Authorization;

namespace TemplateFirebird.Api.Attributes;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{

    private static JsonResult AcessoNaoAutorizado(string erro)
    {
        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Detail = erro
        };
        return new JsonResult(problemDetails) { StatusCode = StatusCodes.Status401Unauthorized };
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        var erro = context.HttpContext.Items[Contexts.ErroToken];
        if (erro != null)
            context.Result = AcessoNaoAutorizado(erro.ToString()!);
        return;
    }
}
