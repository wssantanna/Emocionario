using Emocionario.Application.Usuarios.DTOs;
using FluentValidation;

namespace Emocionario.Api.Validators;

/// <summary>
/// Validador para o DTO de atualização de usuário.
/// </summary>
public class AtualizarUsuarioDtoValidator : AbstractValidator<AtualizarUsuarioDto>
{
    public AtualizarUsuarioDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID é obrigatório.");

        RuleFor(x => x.Nome)
            .Length(3, 50).WithMessage("O nome deve ter entre 3 e 50 caracteres.")
            .Matches("^[a-zA-ZÀ-ÿ\\s]+$").WithMessage("O nome deve conter apenas letras.")
            .When(x => !string.IsNullOrWhiteSpace(x.Nome));

        RuleFor(x => x.Sobrenome)
            .Length(3, 50).WithMessage("O sobrenome deve ter entre 3 e 50 caracteres.")
            .Matches("^[a-zA-ZÀ-ÿ\\s]+$").WithMessage("O sobrenome deve conter apenas letras.")
            .When(x => !string.IsNullOrWhiteSpace(x.Sobrenome));

        RuleFor(x => x.DataNascimento)
            .LessThan(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("A data de nascimento não pode ser uma data futura.")
            .When(x => x.DataNascimento.HasValue);
    }
}
