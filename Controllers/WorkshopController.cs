using Microsoft.AspNetCore.Mvc;
using api_workshops.Interface;
using api_workshops.Models.DTOs;
using api_workshops.Models.DTOs.Read;

namespace api_workshops.Controllers
{
    [ApiController]
    [Route("api/workshops")]
    public class WorkshopController : ControllerBase
    {
        private readonly IWorkshopService _Service;

        public WorkshopController(IWorkshopService workshopService)
        {
            _Service = workshopService;
        }

        [HttpPost]
        public async Task<ActionResult<WorkshopReadDTO>> Create([FromBody] WorkshopCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Dados inválidos.");

            try
            {
                var createdWorkshop = await _Service.Create(dto);
                return Ok(createdWorkshop);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("O nome do workshop não pode ser vazio");
            }
            catch (ArgumentException)
            {
                return BadRequest("A data de realização do workshop não pode ser no passado.");
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Já existe um workshop com este nome.");
            }
            catch (InvalidDataException)
            {
                return BadRequest("Colaborador inválido.");
            }
        }

        [HttpPatch("{workshopId}/colaboradores/{colaboradorId}/presenca")]
        public async Task<IActionResult> AtualizarPresenca(int workshopId, int colaboradorId, [FromBody] PresencaUpdateDTO dto)
        {
            try
            {
                await _Service.AtualizarPresencaAsync(workshopId, colaboradorId, dto.Presente);
                return Ok("Presença atualizada com sucesso.");
            }
            catch (Exception)
            {
                return NotFound("Participação não encontrada.");
            }        
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkshopReadDTO>> GetById(int id)
        {
            try
            {
                var workshop = await _Service.GetById(id);
                return CreatedAtAction(nameof(GetById), new { id = workshop.Id }, workshop);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Nenhum workshop com id {id} foi encontrado!");
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkshopReadDTO>>> GetAll()
        {
            try
            {
                var workshops = await _Service.GetAll();
                return Ok(workshops);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Nenhum workshop cadastrado ainda!");
            }
                
        }

        [HttpGet("name")]
        public async Task<ActionResult<List<WorkshopReadDTO>>> GetByName(string name)
        {
            try
            {
                var workshop = await _Service.GetByName(name);
                return Ok(workshop);
            }
            catch (FileNotFoundException)
            {
                return NotFound($"Nenhum workshop com o nome {name}encontrado com esse nome.");
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WorkshopCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Dados inválidos.");

            try
            {
                await _Service.Update(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Nenhum workshop com id {id} foi encontrado!");
            }
            catch (ArgumentException)
            {
                return BadRequest("Colaborador inválido!");
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _Service.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Nenhum workshop com id {id} foi encontrado!");
            }
        }

    }
}
