using AutoMapper;
using CategoryManagement.Data;
using CategoryManagement.DTOs;
using CategoryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CategoryManagement.Repository
{
    public class CategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public CategoryRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<CategoryDto>> GetAllActive()
        {
            var list = await _context.Categories
                .Where(c => c.IsActive)
                .ToListAsync();

            return _mapper.Map<List<CategoryDto>>(list);
        }

        public async Task<Category?> GetById(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task Insert(Category category)
        {
            category.IsActive = true;
            category.CreatedAt = DateTime.Now;
            category.UpdatedAt = DateTime.Now;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category category)
        {
            var existing = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (existing == null) return;

            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task SoftDelete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return;

            category.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
