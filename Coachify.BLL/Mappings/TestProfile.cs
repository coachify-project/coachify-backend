using AutoMapper;
using Coachify.BLL.DTOs.Test;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<Test, TestDto>()
                .ForMember(d => d.Id,   o => o.MapFrom(s => s.TestId))
                .ReverseMap();   

            CreateMap<CreateTestDto, Test>();
            CreateMap<UpdateTestDto, Test>().ReverseMap();
            CreateMap<CreateTestWithQuestionsDto, Test>();


        }
    }
}
