using HemoControl.Core.Models;
using HemoControl.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace HemoControl.Api.Controllers
{
    [Route("api/[controller]")]
    public class RetiradaController : ControllerBase
    {
        private readonly RetiradaService service;
        private readonly PacienteService pacienteService;

        public RetiradaController(RetiradaService service, PacienteService pacienteService)
        {
            this.service = service;
            this.pacienteService = pacienteService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(service.Listar());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var retirada = service.BuscarId(id);
            if (retirada == null) return NotFound();
            return Ok(retirada);
        }

        [HttpGet("{id}/proxima-retirada")]
        public IActionResult ProximaRetirada(int id)
        {
            var resultado = service.CalcularProximaRetirada(id);
            if (resultado == null) return NotFound();
            return Ok(resultado);
        }

        [HttpPost]
        public IActionResult Post([FromBody] RetiradaRequest req)
        {
            var paciente = pacienteService.BuscarId(req.PacienteId);
            if (paciente == null) return BadRequest("Paciente não encontrado.");

            var retirada = new Retirada
            {
                DataRetirada = req.DataRetirada,
                QuantidadeUI = req.QuantidadeUI,
                Hemocentro   = req.Hemocentro,
                Paciente     = paciente
            };

            var sucesso = service.Criar(retirada, out var erros);
            if (!sucesso) return BadRequest(erros);
            return Ok(retirada);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RetiradaRequest req)
        {
            var retirada = service.BuscarId(id);
            if (retirada == null) return NotFound();

            var paciente = pacienteService.BuscarId(req.PacienteId);
            if (paciente == null) return BadRequest("Paciente não encontrado.");

            retirada.DataRetirada = req.DataRetirada;
            retirada.QuantidadeUI = req.QuantidadeUI;
            retirada.Hemocentro   = req.Hemocentro;
            retirada.Paciente     = paciente;

            var sucesso = service.Atualizar(retirada, out var erros);
            if (!sucesso) return BadRequest(erros);
            return Ok(retirada);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var sucesso = service.Excluir(id);
            if (!sucesso) return NotFound();
            return Ok();
        }
    }

    public record RetiradaRequest(int PacienteId, DateTime DataRetirada, decimal QuantidadeUI, string Hemocentro);
}
