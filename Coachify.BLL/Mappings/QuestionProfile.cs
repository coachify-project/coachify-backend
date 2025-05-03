using AutoMapper;
using Coachify.BLL.DTOs.Question;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<Question, CreateQuestionDto>().ReverseMap();
            CreateMap<Question, UpdateQuestionDto>().ReverseMap();
        }
    }
}