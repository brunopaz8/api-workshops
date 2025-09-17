using api_workshops.Interface;
using api_workshops.Models.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace api_workshops.Controllers
{
    [ApiController]
    [Route("api/colaboradores")]
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorService _service;

        public ColaboradorController(IColaboradorService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ColaboradorCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("O colaborador não pode estar vazio!");

            try
            {
                var novoColaborador = await _service.Create(dto);
                return Ok(novoColaborador);
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Workshop inválido");
            }
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColaboradorReadDTO>> GetById(int id)
        {
            try
            {
                var colaborador = await _service.GetById(id);
                return CreatedAtAction(nameof(GetById), new { id = colaborador.Id }, colaborador);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Nenhum colaborador com id {id} foi encontrado!");
            }
            
        }

        [HttpGet("name")]
        public async Task<ActionResult<List<ColaboradorReadDTO>>> GetByName(string name)
        {
            try
            {
                var colaborador = await _service.GetByName(name);
                return Ok(colaborador);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Nenhum Colaborador com nome {name}");
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<List<ColaboradorReadDTO>>> GetAll()
        {
            try
            {
                var colaborador = await _service.GetAll();
                return Ok(colaborador);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Nenhum colaborador cadastrado!");
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ColaboradorCreateDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("dados inválidos!");
            }

            try
            {
                await _service.Update(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Nenhum colaborador com id {id} foi encontrado!");
            }
            catch (ArgumentException)
            {
                return BadRequest("Workshop inválido!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Nenhum colaborador com id {id} foi encontrado!");
            }
        }
        
    }
}