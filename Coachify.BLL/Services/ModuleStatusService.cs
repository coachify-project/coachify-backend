using AutoMapper;
using Coachify.BLL.DTOs.ModuleStatus;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class ModuleStatusService : IModuleStatusService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ModuleStatusService(ApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }
    public async Task<IEnumerable<ModuleStatusDto>> GetAllAsync() => _mapper.Map<IEnumerable<ModuleStatusDto>>(await _db.ModuleStatuses.ToListAsync());
    public async Task<ModuleStatusDto?> GetByIdAsync(int id) { var e = await _db.ModuleStatuses.FindAsync(id); return e==null? null: _mapper.Map<ModuleStatusDto>(e);}        
    public async Task<ModuleStatusDto> CreateAsync(CreateModuleStatusDto dto) { var e = _mapper.Map<ModuleStatus>(dto); _db.ModuleStatuses.Add(e); await _db.SaveChangesAsync(); return _mapper.Map<ModuleStatusDto>(e);}        
    public async Task UpdateAsync(int id, UpdateModuleStatusDto dto) { var e = await _db.ModuleStatuses.FindAsync(id); if(e==null) return; _mapper.Map(dto,e); await _db.SaveChangesAsync(); }
    public async Task<bool> DeleteAsync(int id) { var e = await _db.ModuleStatuses.FindAsync(id); if(e==null) return false; _db.ModuleStatuses.Remove(e); await _db.SaveChangesAsync(); return true; }
}

