using AutoMapper;
using Coachify.BLL.DTOs.Feedback;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FeedbackId));
            CreateMap<CreateFeedbackDto, Feedback>()
                .ForMember(dest => dest.FeedbackId, opt => opt.Ignore());
            CreateMap<Feedback, UpdateFeedbackDto>().ReverseMap();
        }
    }
}

