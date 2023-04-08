using Exercise4.models;
using Exercise4.models.DTO;
using Exercise4.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;


namespace Exercise4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAnimalRepository _animalRepository;
        public AnimalsController(IConfiguration configuration, IAnimalRepository animal)
        {
            _configuration = configuration;
            _animalRepository = animal;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(string? orderBy)
        {
            var orderByValues = new HashSet<string> { "name", "description", "category", "area" };
            /*if(orderBy is null)
            {
                orderBy = "name";
            }
            orderBy = orderBy ?? "name";*/
            orderBy ??= "name";

            if (!orderByValues.Contains(orderBy))
            {
                orderBy = "name";
            }

            var animals = new List<Animal>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM s22484.dbo.ANIMAL order by {orderBy}";
                await connection.OpenAsync();

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    animals.Add(new Animal
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        description = reader.GetValue(2) == DBNull.Value ? null : reader.GetString(2),
                        category = reader.GetString(3),
                        area = reader.GetString(4)
                    });
                }
            }

            return Ok(animals);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddAnimalDTO animal)
        {

            if (await _animalRepository.Exists(animal.id))
            {
                return Conflict();
            }

            await _animalRepository.Create(new Animal
            {
                id = animal.id,
                name = animal.name,
                description = animal.description,
                category = animal.category,
                area = animal.area,
            });
            return Created($"/api/animals/{animal.id}", animal);
        }
         //api/animals/id
         [HttpPut("{id}")]
         public async Task<IActionResult> Update(int id, UpdateAnimalDTO animal)
         {

             return Ok();
         }
        /* [HttpDelete("{id}")]
         public async Task<IActionResult> Delete(int id)
         {
             return Ok();
         }*/
    }
}
