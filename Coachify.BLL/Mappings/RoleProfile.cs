using AutoMapper;
using Coachify.BLL.DTOs.Role;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                .ReverseMap()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName));

            CreateMap<Role, CreateRoleDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                .ReverseMap()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName));

            CreateMap<Role, UpdateRoleDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                .ReverseMap()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName));
        }
    }
}