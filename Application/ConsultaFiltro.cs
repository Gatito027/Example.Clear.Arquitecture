using AutoMapper;
using Domain.Entities;
using MediatR;
using Domain.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class ConsultaFiltro
    {
        public class LibroUnico : IRequest<LibroMaterialDto>
        {
            public Guid? LibroId { get; set; }
        }

        public class Manejador : IRequestHandler<LibroUnico, LibroMaterialDto>
        {
            private readonly ContextoLibreria _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<LibroMaterialDto> Handle(LibroUnico request, CancellationToken cancellationToken)
            {
                if (request.LibroId == null || request.LibroId == Guid.Empty)
                {
                    throw new ArgumentException("El ID del libro no puede ser nulo o vacío");
                }

                var libro = await _contexto.LibreriaMaterial
                    .FirstOrDefaultAsync(x => x.LibreriaMateriaID == request.LibroId, cancellationToken);

                if (libro == null)
                {
                    throw new KeyNotFoundException("No se encontró el libro especificado");
                }

                var libroDto = _mapper.Map<LibreriaMaterial, LibroMaterialDto>(libro);
                return libroDto;
            }
        }
    }
}
