using AutoMapper;
using Coachify.BLL.DTOs.LessonStatus;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class LessonStatusProfile : Profile
    {
        public LessonStatusProfile()
        {
            CreateMap<LessonStatus, LessonStatusDto>().ReverseMap();
            CreateMap<LessonStatus, CreateLessonStatusDto>().ReverseMap();
            CreateMap<LessonStatus, UpdateLessonStatusDto>().ReverseMap();
        }
    }
}