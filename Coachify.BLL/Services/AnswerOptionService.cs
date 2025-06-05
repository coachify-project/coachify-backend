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

    public async Task<IEnumerable<AnswerOptionDto>> GetAllAsync()
    {
        var options = await _db.AnswerOptions.ToListAsync();
        return _mapper.Map<IEnumerable<AnswerOptionDto>>(options);
    }

    public async Task<IEnumerable<AnswerOptionDto>> GetByQuestionIdAsync(int questionId)
    {
        var options = await _db.AnswerOptions
            .Where(o => o.QuestionId == questionId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<AnswerOptionDto>>(options);
    }

    public async Task<AnswerOptionDto?> GetByIdAsync(int id)
    {
        var option = await _db.AnswerOptions.FindAsync(id);
        if (option == null) return null;
        return _mapper.Map<AnswerOptionDto>(option);
    }

    public async Task<AnswerOptionDto> CreateAsync(CreateAnswerOptionDto dto)
    {
        // Проверяем, что вопрос существует
        var questionExists = await _db.Questions.AnyAsync(q => q.QuestionId == dto.QuestionId);
        if (!questionExists)
            throw new ArgumentException($"Question with id {dto.QuestionId} not found");

        var option = _mapper.Map<AnswerOption>(dto);
        _db.AnswerOptions.Add(option);
        await _db.SaveChangesAsync();

        return _mapper.Map<AnswerOptionDto>(option);
    }

    public async Task<AnswerOptionDto?> UpdateAsync(int id, UpdateAnswerOptionDto dto)
    {
        var option = await _db.AnswerOptions.FindAsync(id);
        if (option == null) return null;

        // Проверяем, если пытаемся сменить QuestionId, что новый вопрос существует
        if (dto.QuestionId != option.QuestionId)
        {
            var questionExists = await _db.Questions.AnyAsync(q => q.QuestionId == dto.QuestionId);
            if (!questionExists)
                throw new ArgumentException($"Question with id {dto.QuestionId} not found");
        }

        _mapper.Map(dto, option);
        await _db.SaveChangesAsync();

        return _mapper.Map<AnswerOptionDto>(option);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var option = await _db.AnswerOptions.FindAsync(id);
        if (option == null) return false;

        _db.AnswerOptions.Remove(option);
        await _db.SaveChangesAsync();

        return true;
    }
}
