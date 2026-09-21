namespace DeskFlow.API.Models
{
    public class Chamado
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public Prioridade Prioridade { get; set; }
        public Status Status { get; set; } = Status.Aberto; // RF06: Atribuir automaticamente "Aberto"
        public string SolicitanteNome { get; set; } = string.Empty;
        public DateTime DataAbertura { get; set; } = DateTime.Now; // RF06: Atribuir data/hora atual

        // Propriedades nulas (serão preenchidas apenas no encerramento - RF08)
        public DateTime? DataFechamento { get; set; }
        public string? Solucao { get; set; }

        // Chave Estrangeira e Relacionamento com Categoria (Exigência RF05)
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        // Relacionamento 1:N com Interação (Exigência RF09)
        public List<Interacao> Interacoes { get; set; } = new();
    }
}