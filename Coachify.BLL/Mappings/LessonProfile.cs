using AutoMapper;
using Coachify.BLL.DTOs.Lesson;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            CreateMap<Lesson, LessonDto>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId));
            CreateMap<CreateLessonDto, Lesson>()
                .ForMember(dest => dest.LessonId, opt => opt.Ignore());
            CreateMap<UpdateLessonDto, Lesson>();
        }
    }
}