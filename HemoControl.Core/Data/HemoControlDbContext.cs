using HemoControl.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HemoControl.Core.Data
{
    public class HemoControlDbContext : DbContext
    {
        public DbSet<Paciente> Pacientes => Set<Paciente>();
        public DbSet<Retirada> Retiradas => Set<Retirada>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = Environment.GetEnvironmentVariable("ConnectionStrings__Default");
            optionsBuilder.UseMySql(conn, ServerVersion.AutoDetect(conn));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var modelPaciente = modelBuilder.Entity<Paciente>();
            modelPaciente.ToTable("pacientes");
            modelPaciente.Property(e => e.Id).HasColumnName("id");
            modelPaciente.Property(e => e.Nome).HasColumnName("nome");
            modelPaciente.Property(e => e.Cpf).HasColumnName("cpf");
            modelPaciente.Property(e => e.TipoHemofilia).HasColumnName("tipo_hemofilia");
            modelPaciente.Property(e => e.DosePrescrita).HasColumnName("dose_prescrita");
            modelPaciente.Property(e => e.AplicacoesPorSemana).HasColumnName("aplicacoes_por_semana");
            modelPaciente.HasKey(e => e.Id);

            var modelRetirada = modelBuilder.Entity<Retirada>();
            modelRetirada.ToTable("retiradas");
            modelRetirada.Property(e => e.Id).HasColumnName("id");
            modelRetirada.Property(e => e.DataRetirada).HasColumnName("data_retirada");
            modelRetirada.Property(e => e.QuantidadeUI).HasColumnName("quantidade_ui");
            modelRetirada.Property(e => e.Hemocentro).HasColumnName("hemocentro");
            modelRetirada.HasOne(e => e.Paciente).WithMany().HasForeignKey("pacienteid");
            modelRetirada.HasKey(e => e.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
