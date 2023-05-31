using Exercise4.Models;
using Exercise4.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Exercise4.Repository
{

    public interface IAnimalRepository
    {
        Task<bool> Exists(int id);
        public Task<List<Animal>> GetAll(string? orderBy);
        public Task AddAnimal(AddAnimalDTO animal);
        public Task UpdateAnimal(int id, UpdateAnimalDTO animal);
        public Task DeleteAnimal(int id);
    }
    public class AnimalRepository : IAnimalRepository
    {
        private readonly IConfiguration _configuration;

        public AnimalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddAnimal(AddAnimalDTO animal)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
                {
                    string query = "INSERT INTO Animal (id, name, description, category, area) VALUES (@1, @2, @3, @4, @5)";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@1", animal.Id);
                    command.Parameters.AddWithValue("@2", animal.Name);
                    command.Parameters.AddWithValue("@3", animal.Description);
                    command.Parameters.AddWithValue("@4", animal.Category);
                    command.Parameters.AddWithValue("@5", animal.Area);

                    await conn.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the animal.", ex);
            }
        }

        public async Task DeleteAnimal(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
                {
                    string query = "DELETE FROM Animal WHERE id = @1";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@1", id);

                    await conn.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the animal.", ex);
            }
        }

        public async Task<bool> Exists(int id)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                SqlCommand command = conn.CreateCommand();
                command.CommandText = $"SELECT id FROM Animal WHERE id = @1";
                command.Parameters.AddWithValue("@1", id);
                await conn.OpenAsync();
                if (await command.ExecuteScalarAsync() is not null)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<Animal>> GetAll(string? orderBy)
        {
            HashSet<string> orderByValues = new HashSet<string> { "name", "description", "category", "area" };
            List<Animal> list = new List<Animal>();

            try
            {
                orderBy ??= "name";
                if (!orderByValues.Contains(orderBy))
                {
                    orderBy = "name";
                }

                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
                {
                    await conn.OpenAsync();

                    string query = $"SELECT * FROM Animal ORDER BY {orderBy}";
                    SqlCommand command = new SqlCommand(query, conn);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Animal animal = new Animal
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Category = reader.GetString(3),
                                Area = reader.GetString(4)
                            };

                            list.Add(animal);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the animals.", ex);
            }
        }


        public async Task UpdateAnimal(int id, UpdateAnimalDTO animal)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
                {
                    string query = "UPDATE Animal SET name = @2, description = @3, category = @4, area = @5 WHERE id = @1";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@1", id);
                    command.Parameters.AddWithValue("@2", animal.Name);
                    command.Parameters.AddWithValue("@3", animal.Description);
                    command.Parameters.AddWithValue("@4", animal.Category);
                    command.Parameters.AddWithValue("@5", animal.Area);

                    await conn.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the animal.", ex);
            }
        }
    }
}
