using AutoMapper;
using Coachify.BLL.DTOs.CoachApplication;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CoachApplicationProfile : Profile
    {
        public CoachApplicationProfile()
        {
            CreateMap<CoachApplication, CoachApplicationDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.SubmittedAt));

            // Create application from user
            CreateMap<CreateCoachApplicationDto, CoachApplication>()
                .ForMember(dest => dest.ApplicationId, opt => opt.Ignore())
                .ForMember(dest => dest.SubmittedAt, opt => opt.Ignore())
                .ForMember(dest => dest.StatusId, opt => opt.Ignore());

            // For status (optional)
            CreateMap<UserCoachApplicationStatus, StatusDto>();
        }
    }
}