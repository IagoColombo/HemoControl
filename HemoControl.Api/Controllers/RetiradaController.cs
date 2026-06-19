using HemoControl.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace HemoControl.Controllers
{
    [Route("api/[controller]")]
    public class RetiradaController : ControllerBase
    {
        private readonly RetiradaService service;

        public RetiradaController(RetiradaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Get() => Ok(service.Listar());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var retirada = service.BuscarId(id);
            return retirada == null ? NotFound() : Ok(retirada);
        }

        [HttpPost]
        public IActionResult Post([FromBody] RetiradaRequest req)
        {
            if (!service.Criar(req, out var erro))
                return BadRequest(erro);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RetiradaRequest req)
        {
            if (!service.Atualizar(id, req, out var erro))
                return BadRequest(erro);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
            => service.Excluir(id) ? Ok() : NotFound();
    }
}
