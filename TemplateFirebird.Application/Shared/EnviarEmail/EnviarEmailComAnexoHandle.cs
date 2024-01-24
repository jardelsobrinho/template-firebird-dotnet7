using MediatR;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace TemplateFirebird.Application.Shared.EnviarEmail;

public class EnviarEmailComAnexoCommand : IRequest<Unit>
{
    public required string Destinatario;
    public required string Assunto;
    public required string Corpo;
    public required byte[] Anexo;
    public required string NomeAnexo;
}

public class EnviarEmailComAnexoHandle : IRequestHandler<EnviarEmailComAnexoCommand, Unit>
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUsername = "";
    private readonly string _smtpPassword = "";
    private readonly string _fromEmail = "";

    public async Task<Unit> Handle(EnviarEmailComAnexoCommand request, CancellationToken cancellationToken)
    {
        var client = new SmtpClient()
        {
            Host = _smtpServer,
            Port = _smtpPort,
            Credentials = new NetworkCredential()
            {
                UserName = _smtpUsername,
                Password = _smtpPassword,
            },
            EnableSsl = true,
            UseDefaultCredentials = false
        };

        var message = new MailMessage(_fromEmail, request.Destinatario, request.Assunto, request.Corpo);
        var stream = new MemoryStream(request.Anexo);
        var attachment = new Attachment(stream, new ContentType("application/pdf"))
        {
            Name = request.NomeAnexo
        };
        message.Attachments.Add(attachment);
        await client.SendMailAsync(message, cancellationToken);
        return Unit.Value;
    }
}
