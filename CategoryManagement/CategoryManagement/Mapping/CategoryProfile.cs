using AutoMapper;
using CategoryManagement.DTOs;
using CategoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CategoryManagement.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
