using MediatR;

namespace TemplateFirebird.Application.OlaMundo;

public class OlaMundoHandler : IRequestHandler<OlaMundoQuery, string>
{
    public Task<string> Handle(OlaMundoQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult("Olá mundo");
    }
}

public record OlaMundoQuery: IRequest<string>;
