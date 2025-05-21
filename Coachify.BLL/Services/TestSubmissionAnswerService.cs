using AutoMapper;
using Coachify.BLL.DTOs.TestSubmissionAnswer;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class TestSubmissionAnswerService : ITestSubmissionAnswerService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public TestSubmissionAnswerService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TestSubmissionAnswerDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<TestSubmissionAnswerDto>>(await _db.TestSubmissionAnswers.ToListAsync());

    public async Task<TestSubmissionAnswerDto?> GetByIdAsync(int id)
    {
        var e = await _db.TestSubmissionAnswers.FindAsync(id);
        return e == null ? null : _mapper.Map<TestSubmissionAnswerDto>(e);
    }

    public async Task<TestSubmissionAnswerDto> CreateAsync(CreateTestSubmissionAnswerDto dto)
    {
        var e = _mapper.Map<TestSubmissionAnswer>(dto);
        _db.TestSubmissionAnswers.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<TestSubmissionAnswerDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateTestSubmissionAnswerDto dto)
    {
        var e = await _db.TestSubmissionAnswers.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.TestSubmissionAnswers.FindAsync(id);
        if (e == null) return false;
        _db.TestSubmissionAnswers.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}