using AutoMapper;
using Coachify.BLL.DTOs.AnswerOption;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class AnswerOptionProfile : Profile
    {
        public AnswerOptionProfile()
        {
            CreateMap<AnswerOption, AnswerOptionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OptionId));
            CreateMap<CreateAnswerOptionDto, AnswerOption>().ReverseMap();
            CreateMap<AnswerOption, UpdateAnswerOptionDto>().ReverseMap();
        }
    }
}