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
            try
            {
                List<Animal> list = await _animalRepository.GetAll(orderBy);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the animals.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddAnimalDTO animal)
        {
            try
            {
                if (await _animalRepository.Exists(animal.Id))
                {
                    return Conflict();
                }

                await _animalRepository.AddAnimal(animal);
                return Created($"api/animals/{animal.Id}", animal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the animal.");
            }
        }


        // PUT: api/animals/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAnimalDTO animal)
        {
            try
            {
                if (await _animalRepository.Exists(id))
                {
                    await _animalRepository.UpdateAnimal(id, animal);
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the animal.");
            }
        }


        // DELETE: api/animals{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _animalRepository.Exists(id))
                {
                    await _animalRepository.DeleteAnimal(id);
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the animal.");
            }
        }


    }

    }
