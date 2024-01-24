using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TemplateFirebird.Application.Shared;
using TemplateFirebird.Domain.Shared;

namespace TemplateFirebird.Application.Auth.ValidaToken;

public class ValidaTokenHandler : IRequestHandler<ValidaTokenCommand, DadosTokenModel>
{
    private readonly AppSettings _appSettings;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICriptografia _criptografia;

    public ValidaTokenHandler(IOptions<AppSettings> appSettings, IUnitOfWork unitOfWork, ICriptografia criptografia)
    {
        _appSettings = appSettings.Value;
        _unitOfWork = unitOfWork;
        _criptografia = criptografia;
    }
    public async Task<DadosTokenModel> Handle(ValidaTokenCommand command, CancellationToken cancellationToken)
    {
        if (command.Token.IsNullOrEmpty())
            throw new BadHttpRequestException("VTH01 - Token não informado");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        try
        {
            tokenHandler.ValidateToken(command.Token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var usuarioNome = _criptografia.Decrypt(jwtToken.Claims.First(x => x.Type == ClaimsConfig.ClaimUser).Value);
            var usuarioCodigo = _criptografia.Decrypt(jwtToken.Claims.First(x => x.Type == ClaimsConfig.ClaimUserId).Value);
            var represenanteCnpj = _criptografia.Decrypt(jwtToken.Claims.First(x => x.Type == ClaimsConfig.ClaimDoc).Value);
            var empresaCnpj = _criptografia.Decrypt(jwtToken.Claims.First(x => x.Type == ClaimsConfig.ClaimDocClient).Value);
            var origem = jwtToken.Claims.First(x => x.Type == ClaimsConfig.ClaimOrigin).Value;
            var claimRefresh = _criptografia.Decrypt(jwtToken.Claims.First(x => x.Type == ClaimsConfig.ClaimRefresh).Value);

            if (command.ValidaTokenRefresh && claimRefresh == "NAO")
            {
                return DadosTokenModel.CriaDadosTokenComErro("JWS02 - Token de refresh inválido");
            }

            if (!command.ValidaTokenRefresh && claimRefresh == "SIM")
            {
                return DadosTokenModel.CriaDadosTokenComErro("JWS04 - Token inválido");
            }

            return new DadosTokenModel()
            {
                UsuarioNome = usuarioNome,
                UsuarioCodigo = int.Parse(usuarioCodigo),
                EmpresaCnpj = empresaCnpj,
                RepresentanteCnpj = represenanteCnpj
            };
        }
        catch (Exception e)
        {
            return DadosTokenModel.CriaDadosTokenComErro(e.Message);
        }
    }
}

public class ValidaTokenCommand : IRequest<DadosTokenModel>
{
    public required string Token { get; set; }
    public required bool ValidaTokenRefresh { get; set; }
}