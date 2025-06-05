using AutoMapper;
using Coachify.BLL.DTOs.AnswerOption;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class AnswerOptionProfile : Profile
    {
        public AnswerOptionProfile()
        {
            CreateMap<AnswerOption, AnswerOptionDto>();
            CreateMap<CreateAnswerOptionDto, AnswerOption>().ReverseMap();
            CreateMap<UpdateAnswerOptionDto, AnswerOption>().ReverseMap();

        }
    }
}