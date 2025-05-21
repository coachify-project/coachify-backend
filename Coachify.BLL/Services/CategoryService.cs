using AutoMapper;
using Coachify.BLL.DTOs.Categoty;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CategoryService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<CategoryDto>>(await _db.Categories.ToListAsync());

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var e = await _db.Categories.FindAsync(id);
        return e == null ? null : _mapper.Map<CategoryDto>(e);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var e = _mapper.Map<Category>(dto);
        _db.Categories.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<CategoryDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var e = await _db.Categories.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Categories.FindAsync(id);
        if (e == null) return false;
        _db.Categories.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}