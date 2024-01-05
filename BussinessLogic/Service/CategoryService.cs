using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLogic.Interface;
using DataAccess.Interface;
using Entity.DTOs;
using Entity.Models;
using Entity.Query;

namespace BussinessLogic.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _data;
        public CategoryService(IUnitOfWork data)
        {
            _data = data;

        }
        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            
            return await _data.Category.GetAll();
        }
        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            QueryOptions<Category> options = new()
            {
                Includes = "Books",
                Where = c => c.CategoryId == id
            };

            return await _data.Category.GetAsync(options);
        }
        public async Task<Category?> GetCategoryByIdAsyncNoTracking(Guid? id)
        {
            return await _data.Category.GetByIdAsync(c => c.CategoryId.Equals(id), includeProperties: "Books");
        }
        public async Task AddCategoryAsync(Category category)
        {
            _data.Category.Add(category);
            await _data.SaveAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _data.Category.Update(category);
            await _data.SaveAsync();
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            _data.Category.Remove(category);
            await _data.SaveAsync();
        }
    }
}
