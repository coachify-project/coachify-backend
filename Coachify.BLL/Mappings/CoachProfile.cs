using AutoMapper;
using Coachify.BLL.DTOs.Certificate;
using Coachify.BLL.DTOs.Coach;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CoachProfile : Profile
    {
        public CoachProfile()
        {
            CreateMap<Coach, CoachDto>()
                .ForMember(dest => dest.CoachId,
                    opt => opt.MapFrom(src => src.CoachId))
                .ForMember(dest => dest.CoachId,
                    opt => opt.MapFrom(src => src.CoachId))
                .ForMember(dest => dest.Bio,
                    opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.Specialization,
                    opt => opt.MapFrom(src => src.Specialization))
                .ForMember(dest => dest.Verified,
                    opt => opt.MapFrom(src => src.Verified));

            CreateMap<CreateCoachDto, Coach>()
                .ForMember(dest => dest.CoachId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            
            CreateMap<UpdateCoachDto, Coach>()
                .ForMember(dest => dest.CoachId,   opt => opt.Ignore())
                .ForMember(dest => dest.User,      opt => opt.Ignore());
        }
    }
}