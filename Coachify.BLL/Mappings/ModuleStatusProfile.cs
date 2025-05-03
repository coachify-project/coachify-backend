using AutoMapper;
using Coachify.BLL.DTOs.ModuleStatus;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class ModuleStatusProfile : Profile
    {
        public ModuleStatusProfile()
        {
            CreateMap<ModuleStatus, ModuleStatusDto>().ReverseMap();
            CreateMap<ModuleStatus, CreateModuleStatusDto>().ReverseMap();
            CreateMap<ModuleStatus, UpdateModuleStatusDto>().ReverseMap();
        }
    }
}