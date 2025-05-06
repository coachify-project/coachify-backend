using AutoMapper;
using Coachify.BLL.DTOs.Role;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public RoleService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<RoleDto>>(await _db.Roles.ToListAsync());

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        var e = await _db.Roles.FindAsync(id);
        return e == null ? null : _mapper.Map<RoleDto>(e);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        var e = _mapper.Map<Role>(dto);
        _db.Roles.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<RoleDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateRoleDto dto)
    {
        var e = await _db.Roles.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Roles.FindAsync(id);
        if (e == null) return false;
        _db.Roles.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}