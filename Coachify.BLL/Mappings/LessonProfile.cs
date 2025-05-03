using AutoMapper;
using Coachify.BLL.DTOs.Lesson;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            CreateMap<Lesson, LessonDto>().ReverseMap();
            CreateMap<Lesson, CreateLessonDto>().ReverseMap();
            CreateMap<Lesson, UpdateLessonDto>().ReverseMap();
        }
    }
}