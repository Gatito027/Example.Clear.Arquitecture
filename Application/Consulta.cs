using MediatR;
using Domain.Entities;
using Domain.Persistencia;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class Consulta : IRequest<List<LibroMaterialDto>>
    {
        public class Ejecuta : IRequest<List<LibroMaterialDto>>
        {
        }

        public class Manejador : IRequestHandler<Ejecuta, List<LibroMaterialDto>>
        {
            private readonly ContextoLibreria _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                _contexto = contexto; 
                _mapper = mapper;
            }

            public async Task<List<LibroMaterialDto>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libros = await _contexto.LibreriaMaterial.ToListAsync(cancellationToken);

                if (libros == null || libros.Count == 0)
                {
                    // Opcional: Maneja el caso en el que no hay libros encontrados.
                    return new List<LibroMaterialDto>();
                }

                var librosDto = _mapper.Map<List<LibreriaMaterial>, List<LibroMaterialDto>>(libros);
                return librosDto; // Asegura que siempre se retorne un valor.
            }
        }
    }
}
