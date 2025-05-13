using AutoMapper;
using Coachify.BLL.DTOs.Coach;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class CoachService : ICoachService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CoachService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CoachDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<CoachDto>>(await _db.Coaches.ToListAsync());

    public async Task<CoachDto?> GetByIdAsync(int id)
    {
        var e = await _db.Coaches.FindAsync(id);
        return e == null ? null : _mapper.Map<CoachDto>(e);
    }

   /* public async Task<CoachDto> CreateAsync(CreateCoachDto dto)
    {
        var e = _mapper.Map<Coach>(dto);
        _db.Coaches.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<CoachDto>(e);
    }*/

    public async Task UpdateAsync(int id, UpdateCoachDto dto)
    {
        var e = await _db.Coaches.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var coach = await _db.Coaches.FindAsync(id);
        if (coach == null) return false;

        var user = await _db.Users.FindAsync(coach.CoachId); 

        _db.Coaches.Remove(coach);
        if (user != null)
            _db.Users.Remove(user);

        await _db.SaveChangesAsync();
        return true;
    }

}