using AutoMapper;
using Coachify.BLL.DTOs.Module;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class ModuleProfile : Profile
    {
        public ModuleProfile()
        {
            CreateMap<Module, ModuleDto>()
                .ForMember(d => d.Status,   o => o.MapFrom(src => src.UserProgresses.FirstOrDefault()!.Status))
                .ForMember(d => d.Lessons,  o => o.MapFrom(src => src.Lessons))
                .ForMember(d => d.Test,     o => o.MapFrom(src => src.Test))
                .ForMember(d => d.Skills,   o => o.MapFrom(src => src.Skills));

            CreateMap<CreateModuleDto, Module>();
            CreateMap<UpdateModuleDto, Module>();

        }
    }
}