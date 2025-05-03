using AutoMapper;
using Coachify.BLL.DTOs.Module;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class ModuleProfile : Profile
    {
        public ModuleProfile()
        {
            CreateMap<Module, ModuleDto>().ReverseMap();
            CreateMap<Module, CreateModuleDto>().ReverseMap();
            CreateMap<Module, UpdateModuleDto>().ReverseMap();
        }
    }
}