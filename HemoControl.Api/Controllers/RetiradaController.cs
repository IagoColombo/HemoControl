using HemoControl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HemoControl.Controllers
{
    [Route("api/[controller]")]
    public class RetiradaController(AppDbContext db) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(db.Retiradas.Include(r => r.Paciente).ToList());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var r = db.Retiradas.Include(r => r.Paciente).FirstOrDefault(r => r.Id == id);
            return r == null ? NotFound() : Ok(r);
        }

        [HttpPost]
        public IActionResult Post([FromBody] RetiradaRequest req)
        {
            if (db.Pacientes.Find(req.PacienteId) is not Paciente paciente)
                return BadRequest("Paciente não encontrado.");

            
            var jaExiste = db.Retiradas.Any(r =>
                r.Paciente.Id == req.PacienteId &&
                r.DataRetirada.Date == req.DataRetirada.Date);

            if (jaExiste)
                return BadRequest("Já existe uma retirada para este paciente nesta data.");

            var retirada = new Retirada
            {
                DataRetirada = req.DataRetirada,
                QuantidadeUI = req.QuantidadeUI,
                Hemocentro   = req.Hemocentro,
                Paciente     = paciente
            };

            db.Retiradas.Add(retirada);
            db.SaveChanges();
            return Ok(retirada);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RetiradaRequest req)
        {
            var retirada = db.Retiradas.Include(r => r.Paciente).FirstOrDefault(r => r.Id == id);
            if (retirada == null) return NotFound();

            if (db.Pacientes.Find(req.PacienteId) is not Paciente paciente)
                return BadRequest("Paciente não encontrado.");

            retirada.DataRetirada = req.DataRetirada;
            retirada.QuantidadeUI = req.QuantidadeUI;
            retirada.Hemocentro   = req.Hemocentro;
            retirada.Paciente     = paciente;

            db.SaveChanges();
            return Ok(retirada);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (db.Retiradas.Find(id) is not Retirada r) return NotFound();
            db.Retiradas.Remove(r);
            db.SaveChanges();
            return Ok();
        }
    }

    public class RetiradaRequest
    {
        public int PacienteId { get; set; }
        public DateTime DataRetirada { get; set; }
        public decimal QuantidadeUI { get; set; }
        public string Hemocentro { get; set; } = "";
    }
}
