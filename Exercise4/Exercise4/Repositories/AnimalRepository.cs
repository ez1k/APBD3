using Exercise4.models;
using Microsoft.Data.SqlClient;

namespace Exercise4.Repositories
{

    public interface IAnimalRepository
    {
        Task<bool> Exists(int id);
        Task Create(Animal animal);
    }

    public class AnimalRepository : IAnimalRepository
    {

        private readonly IConfiguration _configuration;
        public AnimalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Exists(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"select id from animal where id = @1";
                command.Parameters.AddWithValue("@1", id);
                await connection.OpenAsync();
                return await command.ExecuteScalarAsync() is not null;
            }
        }

        public async Task Create(Animal animal)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"insert into animal (id, name, description, category, area) values (@1, @2, @3, @4, @5)";
                command.Parameters.AddWithValue("@1", animal.id);
                command.Parameters.AddWithValue("@2", animal.name);
                command.Parameters.AddWithValue("@3", animal.description == null ? DBNull.Value : animal.description);
                command.Parameters.AddWithValue("@4", animal.category);
                command.Parameters.AddWithValue("@5", animal.area);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
