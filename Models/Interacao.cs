using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DeskFlow.API.Models
{
    [Table("Tb_Interacoes")]
    public class Interacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ChamadoId { get; set; }

        [JsonIgnore]
        public Chamado? Chamado { get; set; }

        [Required]
        [MaxLength(100)]
        public string QuemEscreveu { get; set; } = string.Empty;

        [Required]
        public string Mensagem { get; set; } = string.Empty;

        public DateTime DataRegistro { get; set; } = DateTime.UtcNow;
    }
}