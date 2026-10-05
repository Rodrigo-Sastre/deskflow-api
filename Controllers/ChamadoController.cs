using DeskFlow.API.Models;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DeskFlow.API.Controllers
{
    [Route("api/chamados")]
    [ApiController]
    public class ChamadoController : ControllerBase
    {
        private readonly ChamadoService _service;

        public ChamadoController(ChamadoService service)
        {
            _service = service;
        }

        // RF12: Listagem de chamados com suporte a filtros dinâmicos opcionais via Query String
        [HttpGet]
        public async Task<IActionResult> BuscarTodos([FromQuery] Status? status, [FromQuery] Prioridade? prioridade, [FromQuery] int? categoriaId)
        {
            var chamados = await _service.BuscarTodosAsync(status, prioridade, categoriaId);
            return Ok(chamados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId([FromRoute] int id)
        {
            var chamado = await _service.BuscarPorIdAsync(id);
            if (chamado == null)
                return NotFound(new { mensagem = "Chamado não encontrado." });

            return Ok(chamado);
        }
        // RF06: Endpoint para abertura de chamado (registrado inicialmente como Aberto)
        [HttpPost]
        public async Task<IActionResult> AbrirChamado([FromBody] Chamado chamado)
        {
            var sucesso = await _service.AdicionarAsync(chamado);
            if (!sucesso)
                return BadRequest(new { mensagem = "Dados inválidos. Verifique o SolicitanteNome, Titulo e se a Categoria existe." });

            return CreatedAtAction(nameof(BuscarPorId), new { id = chamado.Id }, chamado);
        }

        // RF07: Altera o status do chamado de Aberto para EmAndamento
        [HttpPatch("{id}/iniciar")]
        public async Task<IActionResult> IniciarAtendimento([FromRoute] int id)
        {
            var (sucesso, mensagem) = await _service.IniciarAtendimentoAsync(id);

            if (!sucesso)
            {
                if (mensagem == "Chamado não encontrado.")
                    return NotFound(new { mensagem });

                return BadRequest(new { mensagem });
            }

            return Ok(new { mensagem });
        }

        // RF08: Encerra o chamado registrando a data atual e a justificativa da solução
        [HttpPatch("{id}/encerrar")]
        public async Task<IActionResult> EncerrarChamado([FromRoute] int id, [FromBody] EncerrarChamadoDto dto)
        {
            var (sucesso, mensagem) = await _service.EncerrarChamadoAsync(id, dto.Solucao);

            if (!sucesso)
            {
                if (mensagem == "Chamado não encontrado.")
                    return NotFound(new { mensagem });

                return BadRequest(new { mensagem });
            }

            return Ok(new { mensagem });
        }

        // RF10: Endpoint aninhado para adicionar notas técnicas e histórico ao chamado
        [HttpPost("{id}/interacoes")]
        public async Task<IActionResult> AdicionarInteracao([FromRoute] int id, [FromBody] CriarInteracaoDto dto)
        {
            var (sucesso, mensagem) = await _service.AdicionarInteracaoAsync(id, dto.Autor, dto.Mensagem);

            if (!sucesso)
            {
                if (mensagem == "Chamado não encontrado.")
                    return NotFound(new { mensagem });

                return BadRequest(new { mensagem });
            }

            return Ok(new { mensagem });
        }
    }
}