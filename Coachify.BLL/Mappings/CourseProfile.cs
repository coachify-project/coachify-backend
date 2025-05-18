using AutoMapper;
using Coachify.BLL.DTOs.Course;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.Modules,
                    opt => opt.MapFrom(src => src.Modules));
            CreateMap<CreateCourseDto, Course>(); 
            CreateMap<Course, UpdateCourseDto>().ReverseMap();
        }
    }
}