using AutoMapper;
using NetKubernetes.Dtos.InmuebleDtos;
using NetKubernetes.Models;

namespace NetKubernetes.Profiles;

public class InmuebleProfile : Profile
{  
    public InmuebleProfile()
    {
        CreateMap<Inmueble, InmuebleResponseDto>();
        CreateMap<InmuebleResponseDto, Inmueble>();

        CreateMap<Inmueble, InmuebleRequestDto>();
        CreateMap<InmuebleRequestDto, Inmueble>();
    }
    
}