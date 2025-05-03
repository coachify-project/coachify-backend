using AutoMapper;
using Coachify.BLL.DTOs.Course;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, CreateCourseDto>().ReverseMap();
            CreateMap<Course, UpdateCourseDto>().ReverseMap();
        }
    }
}