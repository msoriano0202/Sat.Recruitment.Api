using AutoMapper;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Dto.Requests;
using Sat.Recruitment.Dto.Response;
using Sat.Recruitment.Shared;
using System;

namespace Sat.Recruitment.Api.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()));

            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => Enum.Parse<UserType>(src.UserType)));
        }
    }
}
