using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TemplateFirebird.Application.Shared;

namespace TemplateFirebird.Application.Auth.GeraToken;

public class GeraTokenHandler : IRequestHandler<GeraTokenCommand, DadosTokenGeradoModel>
{
    private readonly ICriptografia _criptografia;
    private readonly AppSettings _appSettings;
    public GeraTokenHandler(ICriptografia criptografia, IOptions<AppSettings> appSettings)
    {
        _criptografia = criptografia;
        _appSettings = appSettings.Value;

    }
    public Task<DadosTokenGeradoModel> Handle(GeraTokenCommand command, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        var usuarioNome = _criptografia.Encrypt(command.UsuarioNome);
        var usuarioCodigo = _criptografia.Encrypt(command.UsuarioCodigo.ToString());
        var apiUrl = _criptografia.Encrypt(command.ApiUrl);
        var representanteCnpj = _criptografia.Encrypt(command.RepresentanteCnpj);
        var empresaCnpj = _criptografia.Encrypt(command.EmpresaCnpj);
        var refresh = _criptografia.Encrypt(command.TokenRefresh ? "SIM" : "NAO");
        var dataValidade = DateTime.UtcNow.AddMinutes(10);
        var tokenDataValidade = dataValidade.AddMinutes(-1);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimsConfig.ClaimUser, usuarioNome),
                new Claim(ClaimsConfig.ClaimUserId, usuarioCodigo),
                new Claim(ClaimsConfig.ClaimApi, apiUrl),
                new Claim(ClaimsConfig.ClaimDoc, representanteCnpj),
                new Claim(ClaimsConfig.ClaimDocClient, empresaCnpj),
                new Claim(ClaimsConfig.ClaimOrigin, "SIDI"),
                new Claim(ClaimsConfig.ClaimRefresh, refresh)
            }),
            Expires = command.TokenRefresh ? DateTime.UtcNow.AddHours(48) : dataValidade,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var dadosTokenGerado = new DadosTokenGeradoModel()
        {
            TokenDataValidade = tokenDataValidade,
            Token = tokenHandler.WriteToken(token)
        };
        return Task.FromResult(dadosTokenGerado);
    }
}
public class GeraTokenCommand : IRequest<DadosTokenGeradoModel>
{
    public required int UsuarioCodigo { get; set; }
    public required string UsuarioNome { get; set; }
    public required string EmpresaCnpj { get; set; }
    public required string RepresentanteCnpj { get; set; }
    public required string ApiUrl { get; set; }
    public required bool TokenRefresh { get; set; }

}