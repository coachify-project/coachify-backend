using AutoMapper;
using Coachify.BLL.DTOs.CourseStatus;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CourseStatusProfile : Profile
    {
        public CourseStatusProfile()
        {
            CreateMap<CourseStatus, CourseStatusDto>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
                .ReverseMap()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId));
            CreateMap<CourseStatus, CreateCourseStatusDto>().ReverseMap();
            CreateMap<CourseStatus, UpdateCourseStatusDto>().ReverseMap();
        }
    }
}