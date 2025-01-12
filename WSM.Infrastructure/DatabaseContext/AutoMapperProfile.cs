using AutoMapper;
using WSM.Application.DTOs;

//using ProjectAndTaskManagement.Application.DTOs.Card;
//using ProjectAndTaskManagement.Application.DTOs.Project;
//using ProjectAndTaskManagement.Application.DTOs.User;
//using ProjectAndTaskManagement.Application.DTOs.Workspace;
using WSM.Domain.Entities;
using WSM.Domain.ValueObjects;

namespace WSM.Infrastructure.DatabaseContext
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region User

            //CreateMap<UserReadDto, User>();
            //CreateMap<User, UserReadDto>();

            //CreateMap<UserCreateDto, User>()
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(Convert.ToString(src.Email)))) 
            //    .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => new Phone(Convert.ToString(src.Phone))));
            //CreateMap<User, UserCreateDto>();

            //CreateMap<UserUpdateDto, User>()
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(Convert.ToString(src.Email)))) 
            //    .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => new Phone(Convert.ToString(src.Phone))));

            //CreateMap<User, UserReadDto>();

            #endregion

            #region MikrotikCHR
            CreateMap<MikrotikCHRCreateDto, MikrotikCHR>();
            #endregion

            CreateMap<MikrotikEndpointCreateDto, MikrotikEndpoint>()

    .ForMember(dest => dest.DaysToRenew, opt => opt.MapFrom(src => src.DaysToRenew)) // Map DaysToRenew to DaysToRenew
    .ForMember(dest => dest.AllowedAddress, opt => opt.Ignore()) // Assuming AllowedAddress is set separately
    .ForMember(dest => dest.PublicKey, opt => opt.Ignore()) // Assuming PublicKey is set separately
    .ForMember(dest => dest.PrivateKey, opt => opt.Ignore()) // Assuming PrivateKey is set separately
    .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Assuming UserId is set separately
    .ForMember(dest => dest.MikrotikServerId, opt => opt.Ignore()) // Assuming MikrotikServerId is set separately
    .ForMember(dest => dest.MikrotikInterface, opt => opt.Ignore());  // Assuming MikrotikInterface is set separately


            CreateMap<WgCreateDto, MikrotikEndpoint>();
            CreateMap<MikrotikEndpoint, WgCreateDto>()
    
    .ForMember(dest => dest.AllowedAddress, opt => opt.MapFrom(src => src.AllowedAddress)) // Map AllowedAddress to AllowedAddress
    .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey)) // Map PublicKey to PublicKey
    .ForMember(dest => dest.MikrotikInterface, opt => opt.MapFrom(src => src.MikrotikInterface)) // Map PublicKey to PublicKey

    .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment)); // Map MikrotikInterface to MikrotikInterface
        }
    }
}