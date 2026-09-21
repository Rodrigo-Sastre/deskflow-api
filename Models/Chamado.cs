namespace DeskFlow.API.Models
{
    public class Chamado
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public Prioridade Prioridade { get; set; }
        public Status Status { get; set; } = Status.Aberto;
        public string SolicitanteNome { get; set; } = string.Empty;
        public DateTime DataAbertura { get; set; } = DateTime.Now;

        public DateTime? DataFechamento { get; set; }
        public string? Solucao { get; set; }

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public List<Interacao> Interacoes { get; set; } = new();
    }
}