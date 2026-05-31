using HemoControl.Core.Models;
using HemoControl.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace HemoControl.Api.Controllers
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
        public IActionResult Get()
        {
            return Ok(service.Listar());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var paciente = service.BuscarId(id);
            if (paciente == null) return NotFound();
            return Ok(paciente);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Paciente paciente)
        {
            var sucesso = service.Criar(paciente, out var erros);
            if (!sucesso) return BadRequest(erros);
            return Ok(paciente);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Paciente paciente)
        {
            paciente.Id = id;
            var sucesso = service.Atualizar(paciente, out var erros);
            if (!sucesso) return BadRequest(erros);
            return Ok(paciente);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var sucesso = service.Excluir(id);
            if (!sucesso) return NotFound();
            return Ok();
        }
    }
}
