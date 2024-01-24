using System.ComponentModel.DataAnnotations;

namespace TemplateFirebird.Api.Models.Auths;

public class GeraLoginRequest
{
    [Required(ErrorMessage = "O campo {0} deve ser preenchido")]
    [Display(Name = "Usuário")]
    public required string Usuario { get; set; }

    [Required(ErrorMessage = "O campo {0} deve ser preenchido")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "O campo {0} deve ser preenchido")]
    public required string ApiUrl { get; set; }

    [Required(ErrorMessage = "O campo {0} deve ser preenchido")]
    public required string RepresentanteCnpj { get; set; }
}
