using AutoMapper;
using Coachify.BLL.DTOs.PaymentStatus;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class PaymentStatusProfile : Profile
    {
        public PaymentStatusProfile()
        {
            CreateMap<PaymentStatus, PaymentStatusDto>().ReverseMap();
            CreateMap<PaymentStatus, CreatePaymentStatusDto>().ReverseMap();
            CreateMap<PaymentStatus, UpdatePaymentStatusDto>().ReverseMap();
        }
    }
}