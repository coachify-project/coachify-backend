using AutoMapper;
using Coachify.BLL.DTOs.CoachApplication;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CoachApplicationProfile : Profile
    {
        public CoachApplicationProfile()
        {
            CreateMap<CoachApplication, CoachApplicationDto>().ReverseMap();
            CreateMap<CoachApplication, CreateCoachApplicationDto>().ReverseMap();
            CreateMap<CoachApplication, UpdateCoachApplicationDto>().ReverseMap();
        }
    }
}