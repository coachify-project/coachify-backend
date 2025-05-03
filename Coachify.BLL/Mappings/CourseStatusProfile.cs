using AutoMapper;
using Coachify.BLL.DTOs.CourseStatus;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CourseStatusProfile : Profile
    {
        public CourseStatusProfile()
        {
            CreateMap<CourseStatus, CourseStatusDto>().ReverseMap();
            CreateMap<CourseStatus, CreateCourseStatusDto>().ReverseMap();
            CreateMap<CourseStatus, UpdateCourseStatusDto>().ReverseMap();
        }
    }
}