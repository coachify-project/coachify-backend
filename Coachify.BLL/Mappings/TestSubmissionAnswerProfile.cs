using AutoMapper;
using Coachify.BLL.DTOs.TestSubmissionAnswer;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class TestSubmissionAnswerProfile : Profile
    {
        public TestSubmissionAnswerProfile()
        {
            CreateMap<TestSubmissionAnswer, TestSubmissionAnswerDto>().ReverseMap();
            CreateMap<TestSubmissionAnswer, CreateTestSubmissionAnswerDto>().ReverseMap();
            CreateMap<TestSubmissionAnswer, UpdateTestSubmissionAnswerDto>().ReverseMap();
        }
    }
}