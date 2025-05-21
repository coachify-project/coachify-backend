using AutoMapper;
using Coachify.BLL.DTOs.Categoty;
using Coachify.DAL.Entities;

namespace Coachify.BLL.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.CategoryId));

            // Для создания и обновления
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>(); }
    }
}