using AutoMapper;
using Coachify.BLL.DTOs.Enrollment;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings;

public class EnrollmentStatusProfile : Profile
{
    public EnrollmentStatusProfile()
    {
        CreateMap<EnrollmentStatus, EnrollmentStatusDto>();
    }
}