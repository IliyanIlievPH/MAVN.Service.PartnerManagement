﻿using FluentValidation;
using MAVN.Service.PartnerManagement.Client.Models.Location;
using MAVN.Service.PartnerManagement.Client.Models.Partner;
using MAVN.Service.PartnerManagement.Models.Validation.ContactPerson;

namespace MAVN.Service.PartnerManagement.Models.Validation.Location
{
    public class LocationBaseModelValidation<T> : AbstractValidator<T>
        where T : LocationBaseModel
    {
        public LocationBaseModelValidation()
        {
            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100)
                .WithMessage("The partner name should be present and within a range of 3 to 100 characters long.");

            RuleFor(p => p.Address)
                .MinimumLength(3)
                .MaximumLength(100)
                .WithMessage("The description can be empty or within a range of 3 to 100 characters long.");

            RuleFor(p => p.ContactPerson)
                .SetValidator(new ContactPersonModelValidation());

            RuleFor(l=>l.ExternalId)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(80)
                .WithMessage("The external id should be within a range of 1 to 80 characters long.");

            RuleFor(x => x.AccountingIntegrationCode)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(80)
                .WithMessage("The accounting integration code should be within a range of 1 to 80 characters long.");

            RuleFor(p => p.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude value must be between -90 and 90.");

            RuleFor(p => p.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude value must be between -180 and 180.");
        }
    }
}
