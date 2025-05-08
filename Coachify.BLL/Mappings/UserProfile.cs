using AutoMapper;
using Coachify.BLL.DTOs.User;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore()); 

        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) 
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
        
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
    }
}