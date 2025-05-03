using AutoMapper;
using Coachify.BLL.DTOs.UserCoachApplicationStatus;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class UserCoachApplicationStatusProfile : Profile
    {
        public UserCoachApplicationStatusProfile()
        {
            CreateMap<UserCoachApplicationStatus, UserCoachApplicationStatusDto>().ReverseMap();
            CreateMap<UserCoachApplicationStatus, CreateUserCoachApplicationStatusDto>().ReverseMap();
            CreateMap<UserCoachApplicationStatus, UpdateUserCoachApplicationStatusDto>().ReverseMap();
        }
    }
}