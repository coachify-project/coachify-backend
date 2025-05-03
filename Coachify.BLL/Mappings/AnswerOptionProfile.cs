using AutoMapper;
using Coachify.BLL.DTOs.AnswerOption;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class AnswerOptionProfile : Profile
    {
        public AnswerOptionProfile()
        {
            CreateMap<AnswerOption, AnswerOptionDto>().ReverseMap();
            CreateMap<AnswerOption, CreateAnswerOptionDto>().ReverseMap();
            CreateMap<AnswerOption, UpdateAnswerOptionDto>().ReverseMap();
        }
    }
}