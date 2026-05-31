using System.ComponentModel.DataAnnotations;

namespace HemoControl.Core.Models
{
    public class Retirada
    {
        public int Id { get; set; }

        [Required]
        public DateTime DataRetirada { get; set; }

        [Range(1, 10000000)]
        public decimal QuantidadeUI { get; set; }

        [Required]
        [StringLength(200)]
        public string Hemocentro { get; set; }

        [Required]
        public Paciente Paciente { get; set; }
    }
}
