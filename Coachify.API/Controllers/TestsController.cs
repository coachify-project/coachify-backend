using Coachify.BLL.DTOs.Test;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Coachify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly ITestService _service;
        private readonly ApplicationDbContext _db;

        // Один конструктор, принимающий оба сервиса
        public TestsController(ITestService service, ApplicationDbContext db)
        {
            _service = service;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tests = await _service.GetAllAsync();
            return Ok(tests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var test = await _service.GetByIdAsync(id);
            return test == null ? NotFound() : Ok(test);
        }

        [HttpGet("{testId}/questions")]
        public async Task<IActionResult> GetQuestions(int testId)
        {
            var questions = await _db.Questions
                .Where(q => q.TestId == testId)
                .Include(q => q.Options)
                .Select(q => new
                {
                    q.QuestionId,
                    q.Text,
                    Options = q.Options.Select(o => new
                    {
                        o.OptionId,
                        o.Text
                        // Не отправляем IsCorrect на фронт!
                    })
                })
                .ToListAsync();

            if (!questions.Any())
                return NotFound("Test or questions not found");

            return Ok(questions);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        
        [HttpPost("with-questions")]
        public async Task<ActionResult<TestDto>> CreateWithQuestions([FromBody] CreateTestWithQuestionsDto dto)
        {
            var test = await _service.CreateWithQuestionsAsync(dto);
            return Ok(test);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTestDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
