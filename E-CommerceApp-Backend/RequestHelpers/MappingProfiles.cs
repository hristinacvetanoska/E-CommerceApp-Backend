using AutoMapper;
using E_CommerceApp_Backend.DTOs;
using E_CommerceApp_Backend.Models;

namespace E_CommerceApp_Backend.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProductDto, NewProduct>()
                .ForMember(dest => dest.UserId, act => act.Ignore())
                .ForMember(dest => dest.User, act => act.Ignore())
                .ForMember(dest => dest.SellerName, act => act.Ignore());


            CreateMap<UpdateProductDto, Product>();
            CreateMap<NewProduct, Product>()
                .ForSourceMember(dest => dest.Id, y => y.DoNotValidate())
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.ViewsCounter, act => act.Ignore())
                .ForSourceMember(dest => dest.UserId, y => y.DoNotValidate())
                .ForSourceMember(dest => dest.User, y => y.DoNotValidate());
        }
    }
}
