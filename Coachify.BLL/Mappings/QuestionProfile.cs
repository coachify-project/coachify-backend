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
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));
            CreateMap<CreateQuestionDto, Question>().ReverseMap();
            CreateMap<Question, UpdateQuestionDto>().ReverseMap();
            CreateMap<CreateQuestionWithOptionsDto, Question>();

        }
    }
}