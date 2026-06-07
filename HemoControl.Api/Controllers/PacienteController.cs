using HemoControl.Models;
using Microsoft.AspNetCore.Mvc;

namespace HemoControl.Controllers
{
    [Route("api/[controller]")]
    public class PacienteController(AppDbContext db) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string? nome, string? cpf, int page = 1, int pageSize = 10)
        {
            var query = db.Pacientes.AsQueryable();

            
            if (!string.IsNullOrEmpty(nome))
                query = query.Where(p => p.Nome.Contains(nome));

            
            if (!string.IsNullOrEmpty(cpf))
                query = query.Where(p => p.Cpf.Contains(cpf));

            
            var total = query.Count();
            var pacientes = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new { total, page, pageSize, pacientes });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) => db.Pacientes.Find(id) is Paciente p ? Ok(p) : NotFound();

        [HttpPost]
        public IActionResult Post([FromBody] Paciente paciente)
        {
            db.Pacientes.Add(paciente);
            db.SaveChanges();
            return Ok(paciente);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Paciente paciente)
        {
            paciente.Id = id;
            db.Pacientes.Update(paciente);
            db.SaveChanges();
            return Ok(paciente);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (db.Pacientes.Find(id) is not Paciente p) return NotFound();
            db.Pacientes.Remove(p);
            db.SaveChanges();
            return Ok();
        }
    }
}
