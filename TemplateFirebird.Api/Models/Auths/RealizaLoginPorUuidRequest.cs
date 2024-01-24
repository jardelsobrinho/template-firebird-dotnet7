using System.ComponentModel.DataAnnotations;

namespace TemplateFirebird.Api.Models.Auths;

public class RealizaLoginPorUuidRequest
{
    [Required(ErrorMessage = "O campo {0} deve ser preenchido")]
    public required string Uuid { get; set; }

}
