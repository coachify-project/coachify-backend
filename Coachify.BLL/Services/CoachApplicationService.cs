using AutoMapper;
using Coachify.BLL.DTOs.CoachApplication;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class CoachApplicationService : ICoachApplicationService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CoachApplicationService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CoachApplicationDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<CoachApplicationDto>>(await _db.CoachApplications.ToListAsync());

    public async Task<CoachApplicationDto?> GetByIdAsync(int id)
    {
        var e = await _db.CoachApplications.FindAsync(id);
        return e == null ? null : _mapper.Map<CoachApplicationDto>(e);
    }

    public async Task<CoachApplicationDto> CreateAsync(CreateCoachApplicationDto dto)
    {
        var e = _mapper.Map<CoachApplication>(dto);
        _db.CoachApplications.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<CoachApplicationDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateCoachApplicationDto dto)
    {
        var e = await _db.CoachApplications.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.CoachApplications.FindAsync(id);
        if (e == null) return false;
        _db.CoachApplications.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}