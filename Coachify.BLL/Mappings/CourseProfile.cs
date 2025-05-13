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
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId));
            CreateMap<CreateCourseDto, Course>(); // только в одну сторону
            CreateMap<Course, UpdateCourseDto>().ReverseMap();
        }
    }
}