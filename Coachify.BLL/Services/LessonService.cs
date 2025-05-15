using AutoMapper;
using Coachify.BLL.DTOs.Lesson;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class LessonService : ILessonService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public LessonService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LessonDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<LessonDto>>(await _db.Lessons.ToListAsync());

    public async Task<LessonDto?> GetByIdAsync(int id)
    {
        var e = await _db.Lessons.FindAsync(id);
        return e == null ? null : _mapper.Map<LessonDto>(e);
    }

    public async Task<LessonDto> CreateAsync(CreateLessonDto dto)
    {
        // Пример: преобразование YouTube-ссылки в embed
        string videoUrl = ProcessVideoUrl(dto.VideoUrl);

        var lesson = new Lesson
        {
            Title = dto.Title,
            Introduction = dto.Introduction,
            LessonObjectives = dto.LessonObjectives,
            VideoUrl = videoUrl,
            ModuleId = dto.ModuleId,
            
            
            StatusId = 1
        };

        _db.Lessons.Add(lesson);
        await _db.SaveChangesAsync();
        
        return _mapper.Map<LessonDto>(lesson);
    }
    
    private string ProcessVideoUrl(string url)
    {
        if (url.Contains("youtube.com/watch?v="))
        {
            var videoId = url.Split("v=")[1].Split('&')[0];
            return $"https://www.youtube.com/embed/{videoId}";
        }

        return url;
    }

    public async Task UpdateAsync(int id, UpdateLessonDto dto)
    {
        var e = await _db.Lessons.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Lessons.FindAsync(id);
        if (e == null) return false;
        _db.Lessons.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}