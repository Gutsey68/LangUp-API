using FluentValidation;
using LangUp.DTOs.Auth;
using LangUp.Database;
using Microsoft.EntityFrameworkCore;

namespace LangUp.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(AppDbContext dbContext)
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .MustAsync(async (username, _) =>
                !await dbContext.Users.AnyAsync(u => u.Username == username))
            .WithMessage("Username is already taken");

        RuleFor(x => x.Password)
            .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, _) =>
                !await dbContext.Users.AnyAsync(u => u.Email == email))
            .WithMessage("Email is already taken");
    }
}