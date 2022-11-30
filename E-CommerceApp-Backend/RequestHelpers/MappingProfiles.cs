using AutoMapper;
using E_CommerceApp_Backend.DTOs;
using E_CommerceApp_Backend.Models;

namespace E_CommerceApp_Backend.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
