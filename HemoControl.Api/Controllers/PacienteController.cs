using HemoControl.Core.Models;
using HemoControl.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace HemoControl.Controllers
{
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteService service;

        public PacienteController(PacienteService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Get(string? nome, string? cpf, int page = 1, int pageSize = 10)
            => Ok(service.Listar(nome, cpf, page, pageSize));

        [HttpGet("{id}")]
        public IActionResult Get(int id)
            => service.BuscarId(id) is Paciente p ? Ok(p) : NotFound();

        [HttpPost]
        public IActionResult Post([FromBody] Paciente paciente)
        {
            if (!service.Criar(paciente, out var erros))
                return BadRequest(erros.Select(e => e.ErrorMessage));
            return Ok(paciente);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Paciente paciente)
        {
            paciente.Id = id;
            if (!service.Atualizar(paciente, out var erros))
                return BadRequest(erros.Select(e => e.ErrorMessage));
            return Ok(paciente);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
            => service.Excluir(id) ? Ok() : NotFound();
    }
}
