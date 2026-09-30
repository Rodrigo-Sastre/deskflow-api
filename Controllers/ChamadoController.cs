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

        [HttpGet]
        public async Task<IActionResult> BuscarTodos()
        {
            var chamados = await _service.BuscarTodosAsync();
            return Ok(chamados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var chamado = await _service.BuscarPorIdAsync(id);
            if (chamado == null)
                return NotFound(new { mensagem = "Chamado não encontrado." });

            return Ok(chamado);
        }

        [HttpPost]
        public async Task<IActionResult> AbrirChamado([FromBody] Chamado chamado)
        {
            var sucesso = await _service.AdicionarAsync(chamado);
            if (!sucesso)
                return BadRequest(new { mensagem = "Dados inválidos. Verifique o SolicitanteNome, Titulo e se a Categoria existe." });

            return CreatedAtAction(nameof(BuscarPorId), new { id = chamado.Id }, chamado);
        }

        [HttpPatch("{id}/iniciar")]
        public async Task<IActionResult> IniciarAtendimento(int id)
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


        [HttpPatch("{id}/encerrar")]
        public async Task<IActionResult> EncerrarChamado(int id, [FromBody] EncerrarChamadoDto dto)
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

        [HttpPost("{id}/interacoes")]
        public async Task<IActionResult> AdicionarInteracao(int id, [FromBody] CriarInteracaoDto dto)
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