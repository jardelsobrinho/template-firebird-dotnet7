namespace TemplateFirebird.Infra.Exceptions;

public class EntidadeNaoEncontradaException : Exception
{
    public EntidadeNaoEncontradaException() { }

    public EntidadeNaoEncontradaException(string message) : base(message) { }

    public EntidadeNaoEncontradaException(string message, Exception inner) : base(message, inner) { }
}
