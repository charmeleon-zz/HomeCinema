using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using HomeCinema.Web.Models;

namespace HomeCinema.Web.Infrastructure.Validators
{
    public class MoviewViewModelValidator : AbstractValidator<MovieViewModel>
    {
        public MoviewViewModelValidator()
        {
            RuleFor(m => m.GenreId).GreaterThan(0)
                .WithMessage("Select a Genre");
            RuleFor(m => m.Director).NotEmpty().Length(1, 100)
                .WithMessage("Select a Director");
            RuleFor(m => m.Writer).NotEmpty().Length(1, 50)
                .WithMessage("Select a writer");
            RuleFor(m => m.Producer).NotEmpty().Length(1, 50)
                .WithMessage("Select a producer");
            RuleFor(m => m.Description).NotEmpty()
                .WithMessage("Select a description");
            RuleFor(m => m.Rating).InclusiveBetween((byte)0, (byte)5)
                .WithMessage("Rating must be less than or equal to 5");
            RuleFor(m => m.TrailerURI).NotEmpty().Must(ValidateTrailerURI)
                .WithMessage("Only YouTube trailers are supported.");
        }

        private bool ValidateTrailerURI(string trailerURI)
        {
            return (!string.IsNullOrEmpty(trailerURI) && trailerURI.ToLower().StartsWith("https://www.youtue.com/watch?"));
        }
    }
}