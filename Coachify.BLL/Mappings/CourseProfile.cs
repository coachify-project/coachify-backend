using AutoMapper;
using Coachify.BLL.DTOs.Course;
using Coachify.BLL.DTOs.Module;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CreateCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>().ReverseMap();

        }
    }
}