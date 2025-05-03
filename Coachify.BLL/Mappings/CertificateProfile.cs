using AutoMapper;
using Coachify.BLL.DTOs.Certificate;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CertificateProfile : Profile
    {
        public CertificateProfile()
        {
            CreateMap<Certificate, CertificateDto>().ReverseMap();
            CreateMap<Certificate, CreateCertificateDto>().ReverseMap();
            CreateMap<Certificate, UpdateCertificateDto>().ReverseMap();
        }
    }
}