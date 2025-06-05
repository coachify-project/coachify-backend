using AutoMapper;
using Coachify.BLL.DTOs.Question;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class QuestionService : IQuestionService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public QuestionService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuestionDto>> GetAllAsync()
    {
        var questions = await _db.Questions
            .Include(q => q.Options)
            .ToListAsync();

        return _mapper.Map<IEnumerable<QuestionDto>>(questions);
    }

    public async Task<IEnumerable<QuestionDto>> GetByTestIdAsync(int testId)
    {
        var questions = await _db.Questions
            .Where(q => q.TestId == testId)
            .Include(q => q.Options)
            .ToListAsync();

        return _mapper.Map<IEnumerable<QuestionDto>>(questions);
    }

    public async Task<QuestionDto?> GetByIdAsync(int id)
    {
        var question = await _db.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.QuestionId == id);

        if (question == null) return null;

        return _mapper.Map<QuestionDto>(question);
    }

    public async Task<QuestionDto> CreateAsync(CreateQuestionDto dto)
    {
        var question = _mapper.Map<Question>(dto);
        _db.Questions.Add(question);
        await _db.SaveChangesAsync();

        return _mapper.Map<QuestionDto>(question);
    }

    public async Task<QuestionDto?> UpdateAsync(int id, UpdateQuestionDto dto)
    {
        var question = await _db.Questions.FindAsync(id);
        if (question == null) return null;

        _mapper.Map(dto, question);
        await _db.SaveChangesAsync();

        return _mapper.Map<QuestionDto>(question);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var question = await _db.Questions.FindAsync(id);
        if (question == null) return false;

        _db.Questions.Remove(question);
        await _db.SaveChangesAsync();

        return true;
    }
}
