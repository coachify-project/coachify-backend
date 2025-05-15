using AutoMapper;
using Coachify.BLL.DTOs.Question;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId));
            CreateMap<CreateQuestionDto, Question>().ReverseMap();
            CreateMap<Question, UpdateQuestionDto>().ReverseMap();
        }
    }
}