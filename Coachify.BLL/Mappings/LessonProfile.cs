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
                .ForMember(dest => dest.Status, opt =>
                    opt.MapFrom(src =>
                        src.UserProgresses.FirstOrDefault() != null
                            ? src.UserProgresses.First().Status
                            : null
                    ));

            // Map CreateLessonDto to Lesson
            CreateMap<CreateLessonDto, Lesson>()
                .ForMember(dest => dest.LessonId, opt => opt.Ignore());

            // Map UpdateLessonDto to Lesson
            CreateMap<UpdateLessonDto, Lesson>();
        }
    }
}