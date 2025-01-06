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
        }
    }
}