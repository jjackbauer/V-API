using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolvoApi.Model.DTO;

namespace VolvoApi.Model.Validations
{
    public class UpdateTruckRequestValidation : AbstractValidator<UpdateTruckRequest>
    {
        public UpdateTruckRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The Name of the truck can't be empty")
                .NotNull().WithMessage("The Name of the truck can't be null")
                .MaximumLength(50).WithMessage("The Name of the truck can't have more than 50 characters");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("The Model of the truck can't be empty")
                .NotNull().WithMessage("The Model of the truck can't be null")
                .Matches(@"^(FH|FM)$").WithMessage("The Model of the truck must be either FH or FM");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("The Description of the truck can't be empty")
                .NotNull().WithMessage("The Description of the truck can't be null")
                .MaximumLength(200).WithMessage("The Description of the truck can't have more than 200 characters");

            RuleFor(x => x.ManufacturingYear)
                .NotEmpty().WithMessage("The ManufacturingYear of the truck can't be empty")
                .NotNull().WithMessage("The ManufacturingYear of the truck can't be null")
                .Equal(DateTime.Now.Year).WithMessage($"The ManufacturingYear of the truck must be {DateTime.Now.Year}");

            RuleFor(x => x.ModelYear)
                .NotEmpty().WithMessage("The ModelYear of the truck can't be empty")
                .NotNull().WithMessage("The ModelYear of the truck can't be null")
                .Must(x => x == DateTime.Now.Year || x == DateTime.Now.Year + 1)
                    .WithMessage($"The ModelYear of the truck must be {DateTime.Now.Year} or {DateTime.Now.Year + 1}");
        }
    }
}
