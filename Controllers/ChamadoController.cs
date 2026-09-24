using DeskFlow.API.Models;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChamadoController : ControllerBase
    {
        private readonly ChamadoService _service;

        public ChamadoController(ChamadoService service)
        {
            _service = service;
        }

        // RF12: Listagem (Futuramente receberá as query strings de filtros)
        [HttpGet]
        public async Task<IActionResult> BuscarTodos()
        {
            var chamados = await _service.BuscarTodosAsync();
            return Ok(chamados);
        }

        // RF11: Obter Detalhes Completos do Chamado
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var chamado = await _service.BuscarPorIdAsync(id);
            if (chamado == null)
                return NotFound(new { mensagem = "Chamado não encontrado." });

            return Ok(chamado);
        }

        // RF06: Abrir Novo Chamado
        [HttpPost]
        public async Task<IActionResult> AbrirChamado([FromBody] Chamado chamado)
        {
            var sucesso = await _service.AdicionarAsync(chamado);
            if (!sucesso)
                return BadRequest(new { mensagem = "Dados inválidos. Verifique o SolicitanteNome, Titulo e se a Categoria existe." });

            return CreatedAtAction(nameof(BuscarPorId), new { id = chamado.Id }, chamado);
        }

        // RF07: Iniciar Atendimento
        [HttpPatch("{id}/iniciar")]
        public async Task<IActionResult> IniciarAtendimento(int id)
        {
            var chamado = await _service.BuscarPorIdAsync(id);
            if (chamado == null)
                return NotFound(new { mensagem = "Chamado não encontrado." });

            if (chamado.Status != Status.Aberto)
                return BadRequest(new { mensagem = "Apenas chamados com status 'Aberto' podem ser iniciados." });

            chamado.Status = Status.EmAndamento;
            await _service.AtualizarAsync(chamado);

            return Ok(new { mensagem = "Atendimento iniciado com sucesso.", chamado });
        }

        // RF08: Encerrar Chamado
        [HttpPatch("{id}/encerrar")]
        public async Task<IActionResult> EncerrarChamado(int id, [FromBody] string solucao)
        {
            if (string.IsNullOrWhiteSpace(solucao))
                return BadRequest(new { mensagem = "A solução é obrigatória para encerrar um chamado." });

            var chamado = await _service.BuscarPorIdAsync(id);
            if (chamado == null)
                return NotFound(new { mensagem = "Chamado não encontrado." });

            if (chamado.Status == Status.Fechado)
                return BadRequest(new { mensagem = "Este chamado já se encontra encerrado." });

            chamado.Status = Status.Fechado;
            chamado.DataFechamento = DateTime.UtcNow;
            chamado.Solucao = solucao;

            await _service.AtualizarAsync(chamado);

            return Ok(new { mensagem = "Chamado encerrado com sucesso.", chamado });
        }
    }
}