using Domain.Entities;
using FluentValidation;
using MediatR;
using Domain.Persistencia;

namespace Application
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo { get; set; } = string.Empty; // Evita valores nulos.
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo)
                    .NotEmpty().WithMessage("El título es obligatorio");
                RuleFor(x => x.FechaPublicacion)
                    .NotEmpty().WithMessage("La fecha de publicación es obligatoria");
                RuleFor(x => x.AutorLibro)
                    .NotEmpty().WithMessage("El autor es obligatorio");
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // Validar que las propiedades requeridas ya han sido validadas por FluentValidation.

                var libro = new LibreriaMaterial
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro
                };

                await _contexto.LibreriaMaterial.AddAsync(libro, cancellationToken);
                var resultado = await _contexto.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return Unit.Value; // Indica que la operación fue exitosa.
                }

                throw new InvalidOperationException("No se pudo guardar el libro");
            }
        }
    }
}