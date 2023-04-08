using Exercise4.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Exercise4.Repository
{

    public interface IAnimalRepository
    {
        Task<bool> Exists(int id);
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
    }
}
