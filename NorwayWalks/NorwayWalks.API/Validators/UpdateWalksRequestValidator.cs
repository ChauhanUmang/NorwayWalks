using FluentValidation;
using NorwayWalks.API.Models.DTO;

namespace NorwayWalks.API.Validators
{
    public class UpdateWalksRequestValidator : AbstractValidator<UpdateWalksRequest>
    {
        public UpdateWalksRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
