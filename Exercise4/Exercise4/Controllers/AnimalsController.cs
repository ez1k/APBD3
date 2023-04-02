using Exercise4.models;
using Exercise4.models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;


namespace Exercise4.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AnimalsController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public AnimalsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? orderBy)
        {
            var orderByValues = new HashSet<string> { "name", "description", "category", "area" };
            orderBy = orderBy ?? "name";
            if (orderByValues.Contains(orderBy))
            {
                orderByValues.Add(orderBy);
            }
            var animals = new List<Animal>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"select * from animal order by {orderBy}";
                await connection.OpenAsync();

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync()) {
                    animals.Add(new Animal(
                        reader.GetInt32(0), 
                        reader.GetValue(1) == DBNull.Value ? null : reader.GetString(1),
                        reader.GetString(2), 
                        reader.GetString(3),
                        reader.GetString(4)));

                }
            }
            
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddAnimalDTO animal)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"select * from animals where id = @1";
                command.Parameters.AddWithValue("@1", animal.id);
                await connection.OpenAsync();
                if (await command.ExecuteScalarAsync() is not null)
                {
                    return Conflict;
                }

            }
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
            return Created($"/api/animals/{animal.id}", animal);
        }
        //api/animals/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAnimalDTO animal)
        {
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok();
        }
    }
}
