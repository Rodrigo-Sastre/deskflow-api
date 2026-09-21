namespace DeskFlow.API.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        // Relacionamento 1:N para a entidade Chamado (Exigência RF01)
        public List<Chamado> Chamados { get; set; } = new();
    }
}