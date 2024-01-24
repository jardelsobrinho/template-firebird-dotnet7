using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace TemplateFirebird.Application.Shared;

public class ValidaCnpjCpfHandler : IRequestHandler<ValidaCnpjCpfCommand, Unit>
{
    public Task<Unit> Handle(ValidaCnpjCpfCommand request, CancellationToken cancellationToken)
    {
        if (request.CnpjCpf == "")
        {
            throw new BadHttpRequestException("VCCH01 - CNPJ/CPF deve ser informado");
        }
        var cpfCnpj = LimparCaracteres(request.CnpjCpf);

        if (cpfCnpj.Length != 11 && cpfCnpj.Length != 14)
        {
            throw new BadHttpRequestException("VCCH02 - CNPJ/CPF inválido");
        }

        if (cpfCnpj.Length == 11)
        {
            var cpfValido = ValidarCPF(cpfCnpj);
            if (!cpfValido)
            {
                throw new BadHttpRequestException("VCCH03 - CPF inválido");
            }
        }

        if (cpfCnpj.Length == 14)
        {
            var cnpjValido = ValidarCNPJ(cpfCnpj);
            if (!cnpjValido)
            {
                throw new BadHttpRequestException("VCCH04 - CNPJ inválido");
            }
        }

        return Task.FromResult(Unit.Value);
    }

    private bool ValidarCPF(string cpf)
    {
        cpf = LimparCaracteres(cpf);

        if (cpf.Length != 11)
            return false;

        // Calcula o primeiro dígito verificador
        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += int.Parse(cpf[i].ToString()) * (10 - i);
        int resto = soma % 11;
        int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

        // Calcula o segundo dígito verificador
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(cpf[i].ToString()) * (11 - i);
        resto = soma % 11;
        int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith($"{digitoVerificador1}{digitoVerificador2}");
    }

    private bool ValidarCNPJ(string cnpj)
    {
        cnpj = LimparCaracteres(cnpj);

        if (cnpj.Length != 14)
            return false;

        // Calcula o primeiro dígito verificador
        int soma = 0;
        int[] pesos1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        for (int i = 0; i < 12; i++)
            soma += int.Parse(cnpj[i].ToString()) * pesos1[i];
        int resto = soma % 11;
        int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

        // Calcula o segundo dígito verificador
        soma = 0;
        int[] pesos2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        for (int i = 0; i < 13; i++)
            soma += int.Parse(cnpj[i].ToString()) * pesos2[i];
        resto = soma % 11;
        int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

        return cnpj.EndsWith($"{digitoVerificador1}{digitoVerificador2}");
    }

    private string LimparCaracteres(string input)
    {
        return Regex.Replace(input, @"[^\d]", "");
    }

}

public class ValidaCnpjCpfCommand : IRequest<Unit>
{
    public required string CnpjCpf { get; set; }
}
