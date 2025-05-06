using AutoMapper;
using Coachify.BLL.DTOs.UserCoachApplicationStatus;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class UserCoachApplicationStatusService : IUserCoachApplicationStatusService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UserCoachApplicationStatusService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserCoachApplicationStatusDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<UserCoachApplicationStatusDto>>(await _db.UserCoachApplicationStatuses.ToListAsync());

    public async Task<UserCoachApplicationStatusDto?> GetByIdAsync(int id)
    {
        var e = await _db.UserCoachApplicationStatuses.FindAsync(id);
        return e == null ? null : _mapper.Map<UserCoachApplicationStatusDto>(e);
    }

    public async Task<UserCoachApplicationStatusDto> CreateAsync(CreateUserCoachApplicationStatusDto dto)
    {
        var e = _mapper.Map<UserCoachApplicationStatus>(dto);
        _db.UserCoachApplicationStatuses.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<UserCoachApplicationStatusDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateUserCoachApplicationStatusDto dto)
    {
        var e = await _db.UserCoachApplicationStatuses.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.UserCoachApplicationStatuses.FindAsync(id);
        if (e == null) return false;
        _db.UserCoachApplicationStatuses.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}