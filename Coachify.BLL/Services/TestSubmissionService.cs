using AutoMapper;
using Coachify.BLL.DTOs.TestSubmission;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class TestSubmissionService : ITestSubmissionService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public TestSubmissionService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TestSubmissionDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<TestSubmissionDto>>(await _db.TestSubmissions.ToListAsync());

    public async Task<TestSubmissionDto?> GetByIdAsync(int id)
    {
        var e = await _db.TestSubmissions.FindAsync(id);
        return e == null ? null : _mapper.Map<TestSubmissionDto>(e);
    }

    public async Task<TestSubmissionDto> CreateAsync(CreateTestSubmissionDto dto)
    {
        var e = _mapper.Map<TestSubmission>(dto);
        _db.TestSubmissions.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<TestSubmissionDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateTestSubmissionDto dto)
    {
        var e = await _db.TestSubmissions.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.TestSubmissions.FindAsync(id);
        if (e == null) return false;
        _db.TestSubmissions.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}