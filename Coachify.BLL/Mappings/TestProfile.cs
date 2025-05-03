using AutoMapper;
using Coachify.BLL.DTOs.Test;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<Test, TestDto>().ReverseMap();
            CreateMap<Test, CreateTestDto>().ReverseMap();
            CreateMap<Test, UpdateTestDto>().ReverseMap();
        }
    }
}
