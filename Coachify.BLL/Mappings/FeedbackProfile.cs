using AutoMapper;
using Coachify.BLL.DTOs.Feedback;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackDto>().ReverseMap();
            CreateMap<Feedback, CreateFeedbackDto>().ReverseMap();
            CreateMap<Feedback, UpdateFeedbackDto>().ReverseMap();
        }
    }
}