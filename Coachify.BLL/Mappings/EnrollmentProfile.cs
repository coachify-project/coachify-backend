using AutoMapper;
using Coachify.BLL.DTOs.Enrollment;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
            CreateMap<Enrollment, CreateEnrollmentDto>().ReverseMap();
            CreateMap<Enrollment, UpdateEnrollmentDto>().ReverseMap();
            CreateMap<EnrollmentStatus, EnrollmentStatusDto>();

        }
    }
}