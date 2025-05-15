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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TestId)); ;
            CreateMap<CreateTestDto, Test>().ReverseMap();
            CreateMap<Test, UpdateTestDto>().ReverseMap();
        }
    }
}
