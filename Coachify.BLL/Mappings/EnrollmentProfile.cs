using AutoMapper;
using Coachify.BLL.DTOs.Enrollment;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.EnrollmentId));
            CreateMap<CreateEnrollmentDto, Enrollment>();
            CreateMap<Enrollment, UpdateEnrollmentDto>();
            CreateMap<EnrollmentStatus, EnrollmentStatusDto>();

        }
    }
}