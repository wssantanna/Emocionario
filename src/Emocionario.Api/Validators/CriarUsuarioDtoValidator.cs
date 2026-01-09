using Emocionario.Application.Usuarios.DTOs;
using FluentValidation;

namespace Emocionario.Api.Validators;

/// <summary>
/// Validador para o DTO de criação de usuário.
/// </summary>
public class CriarUsuarioDtoValidator : AbstractValidator<CriarUsuarioDto>
{
    public CriarUsuarioDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 50).WithMessage("O nome deve ter entre 3 e 50 caracteres.")
            .Matches("^[a-zA-ZÀ-ÿ\\s]+$").WithMessage("O nome deve conter apenas letras.");

        RuleFor(x => x.Sobrenome)
            .NotEmpty().WithMessage("O sobrenome é obrigatório.")
            .Length(3, 50).WithMessage("O sobrenome deve ter entre 3 e 50 caracteres.")
            .Matches("^[a-zA-ZÀ-ÿ\\s]+$").WithMessage("O sobrenome deve conter apenas letras.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("O email deve ser um endereço válido.")
            .MaximumLength(255).WithMessage("O email não pode exceder 255 caracteres.");

        RuleFor(x => x.DataNascimento)
            .LessThan(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("A data de nascimento não pode ser uma data futura.")
            .When(x => x.DataNascimento.HasValue);
    }
}
