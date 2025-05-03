using AutoMapper;
using Coachify.BLL.DTOs.Categoty;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
        }
    }
}