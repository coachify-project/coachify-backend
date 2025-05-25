// ModulesController.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Coachify.BLL.Interfaces;
using Coachify.BLL.DTOs;
using Coachify.BLL.DTOs.Module;
using Coachify.BLL.DTOs.Test;

namespace Coachify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _service;

        public ModulesController(IModuleService service)
        {
            _service = service;
        }

        // GET: api/modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetAll()
        {
            var modules = await _service.GetAllAsync();
            return Ok(modules);
        }

        // GET: api/modules/course/5
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetByCourse(int courseId)
        {
            var modules = await _service.GetAllByCourseAsync(courseId);
            return Ok(modules);
        }

        // GET: api/modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDto>> Get(int id)
        {
            var module = await _service.GetByIdAsync(id);
            if (module == null)
                return NotFound();
            return Ok(module);
        }

        // GET: api/modules/5/user/10
        [HttpGet("{moduleId}/user/{userId}")]
        public async Task<ActionResult<ModuleDto>> GetByIdForUser(int moduleId, int userId)
        {
            var module = await _service.GetByIdForUserAsync(moduleId, userId);
            if (module == null)
                return NotFound();
            return Ok(module);
        }

        // GET: api/modules/5/user/10/test
        [HttpGet("{moduleId}/user/{userId}/test")]
        public async Task<ActionResult<TestDto?>> GetTestByModuleForUser(int userId, int moduleId)
        {
            try
            {
                var test = await _service.GetTestByModuleForUserAsync(userId, moduleId);
                return Ok(test);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/modules
        [HttpPost]
        public async Task<ActionResult<ModuleDto>> Create([FromBody] CreateModuleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdModule = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = createdModule.ModuleId }, createdModule);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/modules/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ModuleDto>> Update(int id, [FromBody] UpdateModuleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedModule = await _service.UpdateAsync(id, dto);
                return Ok(updatedModule);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/modules/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}