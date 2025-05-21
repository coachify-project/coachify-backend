using AutoMapper;
using Coachify.BLL.DTOs.FeedbackStatus;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class FeedbackStatusService : IFeedbackStatusService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public FeedbackStatusService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FeedbackStatusDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<FeedbackStatusDto>>(await _db.FeedbackStatuses.ToListAsync());

    public async Task<FeedbackStatusDto?> GetByIdAsync(int id)
    {
        var e = await _db.FeedbackStatuses.FindAsync(id);
        return e == null ? null : _mapper.Map<FeedbackStatusDto>(e);
    }

    public async Task<FeedbackStatusDto> CreateAsync(CreateFeedbackStatusDto dto)
    {
        var e = _mapper.Map<FeedbackStatus>(dto);
        _db.FeedbackStatuses.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<FeedbackStatusDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateFeedbackStatusDto dto)
    {
        var e = await _db.FeedbackStatuses.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.FeedbackStatuses.FindAsync(id);
        if (e == null) return false;
        _db.FeedbackStatuses.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}