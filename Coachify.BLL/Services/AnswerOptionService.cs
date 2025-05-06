using AutoMapper;
using Coachify.BLL.DTOs.AnswerOption;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class AnswerOptionService : IAnswerOptionService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public AnswerOptionService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AnswerOptionDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<AnswerOptionDto>>(await _db.AnswerOptions.ToListAsync());

    public async Task<AnswerOptionDto?> GetByIdAsync(int id)
    {
        var e = await _db.AnswerOptions.FindAsync(id);
        return e == null ? null : _mapper.Map<AnswerOptionDto>(e);
    }

    public async Task<AnswerOptionDto> CreateAsync(CreateAnswerOptionDto dto)
    {
        var e = _mapper.Map<AnswerOption>(dto);
        _db.AnswerOptions.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<AnswerOptionDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateAnswerOptionDto dto)
    {
        var e = await _db.AnswerOptions.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.AnswerOptions.FindAsync(id);
        if (e == null) return false;
        _db.AnswerOptions.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}