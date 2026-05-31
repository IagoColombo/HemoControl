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

        public bool Criar(Retirada retirada, out List<ValidationResult> erros)
        {
            if (!Validar(retirada, out erros))
                return false;

            context.Retiradas.Add(retirada);
            context.SaveChanges();
            return true;
        }

        public bool Atualizar(Retirada retirada, out List<ValidationResult> erros)
        {
            if (!Validar(retirada, out erros))
                return false;

            context.Retiradas.Update(retirada);
            context.SaveChanges();
            return true;
        }

        public List<Retirada> Listar()
        {
            return context.Retiradas.Include(r => r.Paciente).ToList();
        }

        public Retirada? BuscarId(int id)
        {
            return context.Retiradas.Include(r => r.Paciente).Where(r => r.Id == id).FirstOrDefault();
        }

        public bool Excluir(int id)
        {
            var retirada = BuscarId(id);
            if (retirada == null) return false;

            context.Retiradas.Remove(retirada);
            context.SaveChanges();
            return true;
        }

        public object? CalcularProximaRetirada(int id)
        {
            var retirada = BuscarId(id);
            if (retirada == null) return null;

            var paciente = retirada.Paciente;
            int totalDoses = paciente.DosePrescrita > 0
                ? (int)Math.Floor(retirada.QuantidadeUI / paciente.DosePrescrita)
                : 0;

            int diasCobertura = paciente.AplicacoesPorSemana > 0
                ? (int)Math.Ceiling(totalDoses / (double)paciente.AplicacoesPorSemana * 7)
                : 0;

            var proxima = retirada.DataRetirada.AddDays(diasCobertura);
            int diasRestantes = (int)(proxima - DateTime.Today).TotalDays;

            return new
            {
                paciente        = paciente.Nome,
                totalDoses,
                diasCobertura,
                proximaRetirada = proxima.ToString("dd/MM/yyyy"),
                diasRestantes,
                atrasado        = diasRestantes < 0
            };
        }
    }
}
