using FluentValidation;

namespace CRM.Application.Features.Leads.Commands.CreateLead;

public class CreateLeadCommandValidator : AbstractValidator<CreateLeadCommand>
{
    public CreateLeadCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.Source)
            .IsInEnum().WithMessage("Invalid lead source.");

        RuleFor(x => x.AssignedToUserId)
            .NotEmpty().WithMessage("A sales rep must be assigned.");
    }
}