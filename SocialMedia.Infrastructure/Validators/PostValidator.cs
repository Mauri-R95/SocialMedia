using FluentValidation;
using SocialMedia.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Validators
{
    //implementa el paquete fluentValidation para validar los modelos DTO antes de entrar a los repositories
    public class PostValidator : AbstractValidator<PostDto>
    {
        public PostValidator()
        {
            RuleFor(post => post.Description)
                .NotNull()
                .WithMessage("La descripcion no puede ser nula");
            RuleFor(post => post.Description)
                .Length(10, 500).WithMessage("La longitud de la descripcion tiene que ser entre 10 y 500");

            RuleFor(post => post.Date)
                .NotNull()
                .LessThan(DateTime.Now);

        }
    }
}
