using AutoMapper;
using Coachify.BLL.DTOs.Progress;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class ProgressProfile : Profile
    {
        public ProgressProfile()
        {
            CreateMap<ProgressStatus, ProgressStatusDto>().ReverseMap();
        }
    }
}