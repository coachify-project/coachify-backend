using AutoMapper;
using Coachify.BLL.DTOs.Role;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, CreateRoleDto>().ReverseMap();
            CreateMap<Role, UpdateRoleDto>().ReverseMap();
        }
    }
}