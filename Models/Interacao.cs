namespace DeskFlow.API.Models
{
    public class Interacao
    {
        public int Id { get; set; }
        public string Autor { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataRegistro { get; set; } = DateTime.Now;

        // Chave Estrangeira e Relacionamento N:1 com Chamado (Exigência RF09)
        public int ChamadoId { get; set; }
        public Chamado? Chamado { get; set; }
    }
}