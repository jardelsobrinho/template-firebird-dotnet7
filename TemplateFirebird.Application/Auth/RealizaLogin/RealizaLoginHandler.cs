using MediatR;
using Microsoft.AspNetCore.Http;
using TemplateFirebird.Application.Auth.GeraToken;
using TemplateFirebird.Domain.Shared;
using TemplateFirebird.Domain.Usuario;

namespace TemplateFirebird.Application.Auth.RealizaLogin;

public class RealizaLoginHandler : IRequestHandler<RealizaLoginCommand, DadosLoginModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public RealizaLoginHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<DadosLoginModel> Handle(RealizaLoginCommand request, CancellationToken cancellationToken)
    {
        var consultaUsuario = await _unitOfWork.UsuarioRepository.PesquisaAsync(x => x.Usuario.Equals(request.Usuario)
        && x.CnpjRepresentante == request.RepresentanteCnpj);

        if (consultaUsuario.Count == 0)
            throw new BadHttpRequestException("RLH01 - Usuario/Password/Cnpj inválido");

        var usuario = consultaUsuario.First();
        var novoPassword = request.Password;
        if (novoPassword.Value != usuario.Password.Value)
            throw new BadHttpRequestException("RLH02 - Usuario/Password/Cnpj inválido");

        var geraTokenCommand = new GeraTokenCommand()
        {
            ApiUrl = request.ApiUrl,
            RepresentanteCnpj = request.RepresentanteCnpj,
            EmpresaCnpj = "EMPRESA CNPJ"!,
            UsuarioNome = request.Usuario,
            UsuarioCodigo = usuario.Codigo,
            TokenRefresh = false
        };

        var dadosToken = await _mediator.Send(geraTokenCommand, cancellationToken);

        geraTokenCommand.TokenRefresh = true;
        var dadosTokenRefresh = await _mediator.Send(geraTokenCommand, cancellationToken);

        var dadosLogin = new DadosLoginModel()
        {
            Token = dadosToken.Token,
            TokenDataValidade = dadosToken.TokenDataValidade,
            TokenRefresh = dadosTokenRefresh.Token
        };

        return dadosLogin;
    }
}

public class RealizaLoginCommand : IRequest<DadosLoginModel>
{
    public required string Usuario { get; set; }
    public required PasswordValueObject Password { get; set; }
    public required string ApiUrl { get; set; }
    public required string RepresentanteCnpj { get; set; }
}