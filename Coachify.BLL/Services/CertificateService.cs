using AutoMapper;
using Coachify.BLL.DTOs.Certificate;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class CertificateService : ICertificateService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CertificateService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CertificateDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<CertificateDto>>(await _db.Certificates.ToListAsync());

    public async Task<CertificateDto?> GetByIdAsync(int id)
    {
        var e = await _db.Certificates.FindAsync(id);
        return e == null ? null : _mapper.Map<CertificateDto>(e);
    }

    public async Task<CertificateDto> CreateAsync(CreateCertificateDto dto)
    {
        var e = _mapper.Map<Certificate>(dto);
        _db.Certificates.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<CertificateDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateCertificateDto dto)
    {
        var e = await _db.Certificates.FindAsync(id);
        if (e == null) return;
        _mapper.Map(dto, e);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Certificates.FindAsync(id);
        if (e == null) return false;
        _db.Certificates.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}