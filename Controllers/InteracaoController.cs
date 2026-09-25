using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Data;
using DeskFlow.API.Models;

namespace DeskFlow.API.Controllers
{
    // Indica ao .NET que esta classe lida com requisições de API e define a rota padrão
    [ApiController]
    [Route("api/[controller]")]
    public class InteracaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injeção de dependência do banco de dados[cite: 13]
        public InteracaoController(AppDbContext context)
        {
            _context = context;
        }

        // O verbo POST é o padrão para criação de novos recursos no servidor
        [HttpPost]
        public IActionResult CriarInteracao([FromBody] Interacao novaInteracao)
        {
            // Validação de segurança: verificar se o chamado realmente existe no banco
            var chamadoExiste = _context.Chamados.Any(c => c.Id == novaInteracao.ChamadoId);
            if (!chamadoExiste)
            {
                return NotFound("Erro: O chamado informado não existe.");
            }

            // O framework adiciona a entidade na memória e o SaveChanges converte isso num INSERT no banco[cite: 7]
            _context.Interacoes.Add(novaInteracao);
            _context.SaveChanges();

            return Ok(novaInteracao);
        }
    }
}