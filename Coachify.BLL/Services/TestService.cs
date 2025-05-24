using AutoMapper;
using Coachify.BLL.DTOs.Test;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services
{
    public class TestService : ITestService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public TestService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TestDto>> GetAllAsync() =>
            _mapper.Map<IEnumerable<TestDto>>(await _db.Tests.ToListAsync());

        public async Task<TestDto?> GetByIdAsync(int id)
        {
            var test = await _db.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.TestId == id);

            return test == null ? null : _mapper.Map<TestDto>(test);
        }


        public async Task<TestDto> CreateAsync(CreateTestDto dto)
        {
            var e = _mapper.Map<Test>(dto);
            _db.Tests.Add(e);
            await _db.SaveChangesAsync();
            return _mapper.Map<TestDto>(e);
        }
        
        public async Task<TestDto> CreateWithQuestionsAsync(CreateTestWithQuestionsDto dto)
        {
            var test = new Test
            {
                Title = dto.Title,
                ModuleId = dto.ModuleId,
                Questions = dto.Questions.Select(q => new Question
                {
                    Text = q.Text,
                    Options = q.Options.Select(o => new AnswerOption
                    {
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                }).ToList()
            };

            _db.Tests.Add(test);
            await _db.SaveChangesAsync();

            return _mapper.Map<TestDto>(test);
        }


        public async Task UpdateAsync(int id, UpdateTestDto dto)
        {
            var e = await _db.Tests.FindAsync(id);
            if (e == null) return;
            _mapper.Map(dto, e);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _db.Tests.FindAsync(id);
            if (e == null) return false;
            _db.Tests.Remove(e);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}