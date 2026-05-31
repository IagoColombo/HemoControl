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

        public List<Paciente> Listar()
        {
            return context.Pacientes.ToList();
        }

        public Paciente? BuscarId(int id)
        {
            return context.Pacientes.Where(p => p.Id == id).FirstOrDefault();
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
