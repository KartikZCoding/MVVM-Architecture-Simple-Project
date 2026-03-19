using AutoMapper;
using CategoryManagement.DTOs;
using CategoryManagement.Helpers;
using CategoryManagement.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;


namespace CategoryManagement.Repository
{
    public class CategoryRepository
    {
        private readonly IMapper _mapper;

        public CategoryRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<CategoryDto> GetAllActive()
        {
            var list = new List<Category>();

            using var conn = DatabaseHelper.GetConnection();

            const string sql = "SELECT * FROM category WHERE IsActive = 1";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                list.Add(new Category
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Description = reader.GetString("Description"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt"),
                    IsActive = reader.GetBoolean("IsActive")
                });
            }
            return _mapper.Map<List<CategoryDto>>(list);
        }

        public Category? GetById(int id)
        {
            using var conn = DatabaseHelper.GetConnection();

            const string sql = "SELECT * FROM category WHERE Id = @id AND IsActive = 1";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Category
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Description = reader.GetString("Description"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt"),
                    IsActive = reader.GetBoolean("IsActive")
                };
            }
            return null;
        }

        public void Insert(Category category)
        {
            using var conn = DatabaseHelper.GetConnection();

            const string sql = "INSERT INTO category (Name, Description, IsActive, CreatedAt, UpdatedAt) VALUES (@Name, @Description, 1, NOW(), NOW())";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", category.Name);
            cmd.Parameters.AddWithValue("@Description", category.Description);

            cmd.ExecuteNonQuery();
        }

        public void Update(Category category)
        {
            using var conn = DatabaseHelper.GetConnection();

            const string sql = "UPDATE category SET Name = @Name, Description = @Description, UpdatedAt = NOW() WHERE Id = @Id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", category.Name);
            cmd.Parameters.AddWithValue("@Description", category.Description);
            cmd.Parameters.AddWithValue("@Id", category.Id);

            cmd.ExecuteNonQuery();
        }

        public void SoftDelete(int id)
        {
            using var conn = DatabaseHelper.GetConnection();

            const string sql = "UPDATE category SET IsActive = 0 WHERE Id = @Id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
