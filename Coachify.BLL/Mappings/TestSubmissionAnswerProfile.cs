using AutoMapper;
using Coachify.BLL.DTOs.TestSubmissionAnswer;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class TestSubmissionAnswerProfile : Profile
    {
        public TestSubmissionAnswerProfile()
        {
            CreateMap<TestSubmissionAnswer, TestSubmissionAnswerDto>();
            CreateMap<CreateTestSubmissionAnswerDto, TestSubmissionAnswer>();
            CreateMap<UpdateTestSubmissionAnswerDto, TestSubmissionAnswer>();

        }
    }
}