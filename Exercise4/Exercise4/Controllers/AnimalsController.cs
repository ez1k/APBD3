using Exercise4.Models;
using Exercise4.Models.DTO;
using Exercise4.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace Exercise4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly IAnimalRepository _animalRepository;

        public AnimalsController(IConfiguration configuration, IAnimalRepository animalRepository)
        {
            _configuration = configuration;
            _animalRepository = animalRepository;
        }

        // GET: api/animals
        [HttpGet]
        public async Task<IActionResult> GetAll(String? orderBy)
        {
            HashSet<string> orderByValues = new HashSet<string> { "name", "description", "category", "area" };
            List<Animal> list = new List<Animal>();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                orderBy ??= "name";
                if (!orderByValues.Contains(orderBy))
                {
                    orderBy = "name";
                }
                string query = $"SELECT * FROM Animal ORDER BY {orderBy}";
                SqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                await conn.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Animal
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetValue(2) == DBNull.Value ? null : reader.GetString(2),
                            Category = reader.GetString(3),
                            Area = reader.GetString(4)
                        });
                    }
                }
            }
            return Ok(list);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddAnimalDTO animal)
        {
            if (await _animalRepository.Exists(animal.Id))
            {
                return Conflict();
            }

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                string query = "INSERT INTO Animal (id, name, description, category, area) values (@1, @2, @3, @4, @5)";
                SqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@1", animal.Id);
                command.Parameters.AddWithValue("@2", animal.Name);
                command.Parameters.AddWithValue("@3", animal.Description);
                command.Parameters.AddWithValue("@4", animal.Category);
                command.Parameters.AddWithValue("@5", animal.Area);
                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            return Created($"api/animals/{animal.Id}", animal);
        }

               
            // PUT: api/animals/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, UpdateAnimalDTO animal)
            {
                if (await _animalRepository.Exists(id))
                {

                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
                {
                    string query = "Update Animal set name = @2,  description = @3,  category = @4,  area = @5 where id = @1";
                    SqlCommand command = conn.CreateCommand();
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@1", id);
                    command.Parameters.AddWithValue("@2", animal.Name);
                    command.Parameters.AddWithValue("@3", animal.Description);
                    command.Parameters.AddWithValue("@4", animal.Category);
                    command.Parameters.AddWithValue("@5", animal.Area);
                    await conn.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                return Ok();
                }

            return NotFound();
        }

            // DELETE: api/animals{id}
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                if (await _animalRepository.Exists(id))
                {

                    using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
                    {
                        string query = "Delete from Animal  where id = @1";
                        SqlCommand command = conn.CreateCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@1", id);
                        await conn.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    return Ok();
                }

                return NotFound();
            
            }
            

        }

    }
