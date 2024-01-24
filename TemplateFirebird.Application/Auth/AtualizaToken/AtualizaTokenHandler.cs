using MediatR;
using Microsoft.AspNetCore.Http;
using TemplateFirebird.Application.Auth.RealizaLogin;
using TemplateFirebird.Application.Auth.ValidaToken;
using TemplateFirebird.Domain.Shared;

namespace TemplateFirebird.Application.Auth.AtualizaToken;

public class AtualizaTokenHandler : IRequestHandler<AtualizaTokenCommand, DadosLoginModel>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    public AtualizaTokenHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<DadosLoginModel> Handle(AtualizaTokenCommand request, CancellationToken cancellationToken)
    {
        var validaToken = new ValidaTokenCommand()
        {
            Token = request.TokenRefresh,
            ValidaTokenRefresh = true
        };
        var dadosToken = await _mediator.Send(validaToken);
        if (dadosToken.Erro != "" && dadosToken.Erro != null)
            throw new BadHttpRequestException($"ATH02 - Token refresh invalido - {dadosToken.Erro}");

        var usuario = await _unitOfWork.UsuarioRepository.CarregaDadosAsync(x => x.Codigo == dadosToken.UsuarioCodigo)
                ?? throw new BadHttpRequestException("ATH01 - Usuário não encontrado");

        var realizaLoginCommand = new RealizaLoginCommand()
        {
            ApiUrl = request.ApiUrl,
            Password = usuario.Password,
            Usuario = dadosToken.UsuarioNome,
            RepresentanteCnpj = dadosToken.RepresentanteCnpj
        };

        return await _mediator.Send(realizaLoginCommand);
    }
}

public class AtualizaTokenCommand : IRequest<DadosLoginModel>
{
    public required string TokenRefresh { get; set; }
    public required string ApiUrl { get; set; }
}
