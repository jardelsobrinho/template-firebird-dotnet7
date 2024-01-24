using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateFirebird.Application.OlaMundo;

namespace TemplateFirebird.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/ola-mundo")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ApiController]
    public class OlaMundoController : DefaultController
    {

        private readonly IMediator _mediator;
        public OlaMundoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna olá mundo
        /// </summary>
        /// <response code="200">olá mundo</response> 
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet()]
        public async Task<IActionResult> RetornaOlaMundoAsync()
        {
            var query = new OlaMundoQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// Acesso autorizado
        /// </summary>
        /// <response code="200">Acesso autorizado</response> 
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpGet("teste-auth")]
        public IActionResult AcessoAuthorizado()
        {
            return Ok("Acesso autorizado");
        }

    }
}
