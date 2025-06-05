using AutoMapper;
using Coachify.BLL.DTOs.TestSubmissionAnswer;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class TestSubmissionAnswerService : ITestSubmissionAnswerService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public TestSubmissionAnswerService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TestSubmissionAnswerDto>> GetAllAsync()
    {
        var answers = await _db.TestSubmissionAnswers.ToListAsync();
        return _mapper.Map<IEnumerable<TestSubmissionAnswerDto>>(answers);
    }

    public async Task<TestSubmissionAnswerDto?> GetByIdAsync(int id)
    {
        var entity = await _db.TestSubmissionAnswers.FindAsync(id);
        return entity == null ? null : _mapper.Map<TestSubmissionAnswerDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.TestSubmissionAnswers.FindAsync(id);
        if (entity == null) return false;

        _db.TestSubmissionAnswers.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}