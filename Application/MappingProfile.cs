using AutoMapper;
using Domain.Entities;

namespace Application
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LibreriaMaterial, LibroMaterialDto>();
        }
    }
}
