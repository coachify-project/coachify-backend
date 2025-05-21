using AutoMapper;
using Coachify.BLL.DTOs.Payment;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public PaymentService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<PaymentDto>>(await _db.Payments.ToListAsync());

    public async Task<PaymentDto?> GetByIdAsync(int id)
    {
        var e = await _db.Payments.FindAsync(id);
        return e == null ? null : _mapper.Map<PaymentDto>(e);
    }

    public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
    {
        var e = _mapper.Map<Payment>(dto);
        _db.Payments.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<PaymentDto>(e);
    }

    public async Task UpdateAsync(int id, UpdatePaymentDto dto)
    {
        var e = await _db.Payments.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Payments.FindAsync(id);
        if (e == null) return false;
        _db.Payments.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}