using Exercise4.models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Exercise4.Repositories
{
    public interface IAnimalRepository
    {
        public void Create();
    }
    public class AnimalRepository : IAnimalRepository
    {
        private readonly IConfiguration _configuration;

        public AnimalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Create()
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
                await command.ExecuteReaderAsync();


            }
        }

        
    }
}
