using AutoMapper;
using Coachify.BLL.DTOs.Question;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class QuestionService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public QuestionService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuestionDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<QuestionDto>>(await _db.Questions.ToListAsync());

    public async Task<QuestionDto?> GetByIdAsync(int id)
    {
        var e = await _db.Questions.FindAsync(id);
        return e == null ? null : _mapper.Map<QuestionDto>(e);
    }

    public async Task<QuestionDto> CreateAsync(CreateQuestionDto dto)
    {
        var e = _mapper.Map<Question>(dto);
        _db.Questions.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<QuestionDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateQuestionDto dto)
    {
        var e = await _db.Questions.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Questions.FindAsync(id);
        if (e == null) return false;
        _db.Questions.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}