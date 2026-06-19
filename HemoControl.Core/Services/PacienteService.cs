using HemoControl.Core.Data;
using HemoControl.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace HemoControl.Core.Services
{
    public class PacienteService
    {
        private readonly HemoControlDbContext context;

        public PacienteService(HemoControlDbContext context)
        {
            this.context = context;
        }

        public bool Validar(Paciente paciente, out List<ValidationResult> erros)
        {
            var validation = new ValidationContext(paciente);
            erros = new List<ValidationResult>();
            Validator.TryValidateObject(paciente, validation, erros, true);
            return erros.Count == 0;
        }

        public object Listar(string? nome, string? cpf, int page, int pageSize)
        {
            var query = context.Pacientes.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(p => p.Nome.Contains(nome));

            if (!string.IsNullOrEmpty(cpf))
                query = query.Where(p => p.Cpf.Contains(cpf));

            var total = query.Count();
            var pacientes = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new { total, page, pageSize, pacientes };
        }

        public Paciente? BuscarId(int id)
        {
            return context.Pacientes.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool Criar(Paciente paciente, out List<ValidationResult> erros)
        {
            if (!Validar(paciente, out erros))
                return false;

            context.Pacientes.Add(paciente);
            context.SaveChanges();
            return true;
        }

        public bool Atualizar(Paciente paciente, out List<ValidationResult> erros)
        {
            if (!Validar(paciente, out erros))
                return false;

            context.Pacientes.Update(paciente);
            context.SaveChanges();
            return true;
        }

        public bool Excluir(int id)
        {
            var paciente = BuscarId(id);
            if (paciente == null) return false;

            context.Pacientes.Remove(paciente);
            context.SaveChanges();
            return true;
        }
    }
}
