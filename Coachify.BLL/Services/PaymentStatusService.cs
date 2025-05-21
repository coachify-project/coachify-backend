using AutoMapper;
using Coachify.BLL.DTOs.PaymentStatus;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class PaymentStatusService : IPaymentStatusService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public PaymentStatusService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PaymentStatusDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<PaymentStatusDto>>(await _db.PaymentStatuses.ToListAsync());

    public async Task<PaymentStatusDto?> GetByIdAsync(int id)
    {
        var e = await _db.PaymentStatuses.FindAsync(id);
        return e == null ? null : _mapper.Map<PaymentStatusDto>(e);
    }

    public async Task<PaymentStatusDto> CreateAsync(CreatePaymentStatusDto dto)
    {
        var e = _mapper.Map<PaymentStatus>(dto);
        _db.PaymentStatuses.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<PaymentStatusDto>(e);
    }

    public async Task UpdateAsync(int id, UpdatePaymentStatusDto dto)
    {
        var e = await _db.PaymentStatuses.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.PaymentStatuses.FindAsync(id);
        if (e == null) return false;
        _db.PaymentStatuses.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}