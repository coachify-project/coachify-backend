using AutoMapper;
using Coachify.BLL.DTOs.TestSubmission;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class TestSubmissionProfile : Profile
    {
        public TestSubmissionProfile()
        {
            CreateMap<TestSubmission, TestSubmissionDto>().ReverseMap();
            CreateMap<TestSubmission, CreateTestSubmissionDto>().ReverseMap();
            CreateMap<TestSubmission, UpdateTestSubmissionDto>().ReverseMap();
        }
    }
}