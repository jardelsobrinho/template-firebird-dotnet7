using Microsoft.AspNetCore.Mvc;
using TemplateFirebird.Api.Authorization;
using TemplateFirebird.Application.Auth;

namespace TemplateFirebird.Api.Controllers;

[ApiController]
public class DefaultController : ControllerBase
{
    protected DadosTokenModel DadosToken => (DadosTokenModel)HttpContext.Items[Contexts.DadosToken]!;
}
