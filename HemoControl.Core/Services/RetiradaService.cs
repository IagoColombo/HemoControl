using HemoControl.Core.Data;
using HemoControl.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HemoControl.Core.Services
{
    public class RetiradaService
    {
        private readonly HemoControlDbContext context;

        public RetiradaService(HemoControlDbContext context)
        {
            this.context = context;
        }

        public bool Validar(Retirada retirada, out List<ValidationResult> erros)
        {
            var validation = new ValidationContext(retirada);
            erros = new List<ValidationResult>();
            Validator.TryValidateObject(retirada, validation, erros, true);
            return erros.Count == 0;
        }

        public List<Retirada> Listar()
        {
            return context.Retiradas.Include(r => r.Paciente).ToList();
        }

        public Retirada? BuscarId(int id)
        {
            return context.Retiradas.Include(r => r.Paciente).Where(r => r.Id == id).FirstOrDefault();
        }

        public bool Criar(RetiradaRequest req, out string? erro)
        {
            erro = null;

            var paciente = context.Pacientes.Find(req.PacienteId);
            if (paciente == null)
            {
                erro = "Paciente não encontrado.";
                return false;
            }

            
            var jaExiste = context.Retiradas.Any(r =>
                r.Paciente.Id == req.PacienteId &&
                r.DataRetirada.Date == req.DataRetirada.Date);

            if (jaExiste)
            {
                erro = "Já existe uma retirada para este paciente nesta data.";
                return false;
            }

            var retirada = new Retirada
            {
                DataRetirada = req.DataRetirada,
                QuantidadeUI = req.QuantidadeUI,
                Hemocentro   = req.Hemocentro,
                Paciente     = paciente
            };

            if (!Validar(retirada, out var erros))
            {
                erro = string.Join(", ", erros.Select(e => e.ErrorMessage));
                return false;
            }

            context.Retiradas.Add(retirada);
            context.SaveChanges();
            return true;
        }

        public bool Atualizar(int id, RetiradaRequest req, out string? erro)
        {
            erro = null;

            var retirada = BuscarId(id);
            if (retirada == null)
            {
                erro = "Retirada não encontrada.";
                return false;
            }

            var paciente = context.Pacientes.Find(req.PacienteId);
            if (paciente == null)
            {
                erro = "Paciente não encontrado.";
                return false;
            }

            retirada.DataRetirada = req.DataRetirada;
            retirada.QuantidadeUI = req.QuantidadeUI;
            retirada.Hemocentro   = req.Hemocentro;
            retirada.Paciente     = paciente;

            if (!Validar(retirada, out var erros))
            {
                erro = string.Join(", ", erros.Select(e => e.ErrorMessage));
                return false;
            }

            context.SaveChanges();
            return true;
        }

        public bool Excluir(int id)
        {
            var retirada = BuscarId(id);
            if (retirada == null) return false;

            context.Retiradas.Remove(retirada);
            context.SaveChanges();
            return true;
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
