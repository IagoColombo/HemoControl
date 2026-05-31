using System.ComponentModel.DataAnnotations;

namespace HemoControl.Core.Models
{
    public class Paciente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14)]
        public string Cpf { get; set; }

        [Required]
        public string TipoHemofilia { get; set; }

        [Range(1, 1000000)]
        public decimal DosePrescrita { get; set; }

        [Range(1, 7)]
        public int AplicacoesPorSemana { get; set; }
    }
}
