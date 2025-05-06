using AutoMapper;
using Coachify.BLL.DTOs.User;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UserService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<UserDto>>(await _db.Users.ToListAsync());

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var u = await _db.Users.FindAsync(id);
        return u == null ? null : _mapper.Map<UserDto>(u);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var e = _mapper.Map<User>(dto);
        _db.Users.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<UserDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateUserDto dto)
    {
        var e = await _db.Users.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Users.FindAsync(id);
        if (e == null) return false;
        _db.Users.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}