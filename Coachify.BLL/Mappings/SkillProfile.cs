using AutoMapper;
using Coachify.BLL.DTOs.Skill;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings;

public class SkillProfile : Profile
{
    public SkillProfile()
    {
        CreateMap<Skill, SkillDto>();
        
    }
}