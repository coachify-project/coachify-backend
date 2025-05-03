using AutoMapper;
using Coachify.BLL.DTOs.FeedbackStatus;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class FeedbackStatusProfile : Profile
    {
        public FeedbackStatusProfile()
        {
            CreateMap<FeedbackStatus, FeedbackStatusDto>().ReverseMap();
            CreateMap<FeedbackStatus, CreateFeedbackStatusDto>().ReverseMap();
            CreateMap<FeedbackStatus, UpdateFeedbackStatusDto>().ReverseMap();
        }
    }
}