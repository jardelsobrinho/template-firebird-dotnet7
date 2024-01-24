namespace TemplateFirebird.Domain.Shared;

public class EntidadeInvalidaException : Exception
{
    public EntidadeInvalidaException() { }

    public EntidadeInvalidaException(string message) : base(message) { }

    public EntidadeInvalidaException(string message, Exception inner) : base(message, inner) { }
}
