using AutoMapper;
using Coachify.BLL.DTOs.Feedback;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class FeedbackService : IFeedbackService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public FeedbackService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FeedbackDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<FeedbackDto>>(await _db.Feedbacks.ToListAsync());

    public async Task<FeedbackDto?> GetByIdAsync(int id)
    {
        var e = await _db.Feedbacks.FindAsync(id);
        return e == null ? null : _mapper.Map<FeedbackDto>(e);
    }

    public async Task<FeedbackDto> CreateAsync(CreateFeedbackDto dto)
    {
        var e = _mapper.Map<Feedback>(dto);
        _db.Feedbacks.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<FeedbackDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateFeedbackDto dto)
    {
        var e = await _db.Feedbacks.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Feedbacks.FindAsync(id);
        if (e == null) return false;
        _db.Feedbacks.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}