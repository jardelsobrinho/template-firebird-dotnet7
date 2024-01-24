using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateFirebird.Api.Models.Auths;
using TemplateFirebird.Application.Auth;
using TemplateFirebird.Application.Auth.AtualizaToken;
using TemplateFirebird.Application.Auth.GeraToken;
using TemplateFirebird.Application.Auth.RealizaLogin;
using TemplateFirebird.Domain.Usuario;

namespace TemplateFirebird.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/auth")]
public class AuthController : DefaultController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Realiza o login do usuário
    /// </summary>
    /// <response code="200">Retorna o token</response> 
    [ProducesResponseType(typeof(DadosTokenGeradoModel), StatusCodes.Status200OK)]
    [HttpPost("login")]
    public async Task<IActionResult> RealizaLoginAsync([FromBody] GeraLoginRequest request)
    {
        var command = new RealizaLoginCommand()
        {
            ApiUrl = request.ApiUrl,
            Password = new PasswordValueObject(request.Password),
            Usuario = request.Usuario,
            RepresentanteCnpj = request.RepresentanteCnpj
        };

        var dadosTokenGerado = await _mediator.Send(command);
        return Ok(dadosTokenGerado);
    }

    /// <summary>
    /// Atualiza token
    /// </summary>
    /// <response code="200">Token token atualizado</response> 
    [HttpPost("atualiza-token")]
    [ProducesResponseType(typeof(DadosTokenModel), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> AtualizaTokenAsync(AtualizaTokenRequest request)
    {
        var command = new AtualizaTokenCommand()
        {
            ApiUrl = "",
            TokenRefresh = request.TokenRefresh,
        };

        var dadosToken = await _mediator.Send(command);
        return Ok(dadosToken);
    }

    [HttpGet]
    public IActionResult TesteAuth()
    {
        return Ok("ACESSO AUTORIZADO");
    }
}
