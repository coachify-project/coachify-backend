using AutoMapper;
using Coachify.BLL.DTOs.Payment;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Payment, CreatePaymentDto>().ReverseMap();
            CreateMap<Payment, UpdatePaymentDto>().ReverseMap();
        }
    }
}