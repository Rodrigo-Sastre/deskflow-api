using DeskFlow.API.Models;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers
{

    // RNF02: Rota RESTful no plural e minúsculo
    [Route("api/categoria")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _service;


        public CategoriaController(CategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodas()
        {
            var categorias = await _service.BuscarTodasAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var categoria = await _service.BuscarPorIdAsync(id);
            if (categoria == null)
                return NotFound(new { mensagem = "Categoria não encontrada." });

            return Ok(categoria);
        }

        // RF02: Cadastra nova categoria e retorna o status 201 Created com a rota de consulta
        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] Categoria categoria)
        {
            var sucesso = await _service.AdicionarAsync(categoria);
            if (!sucesso)
                return BadRequest(new { mensagem = "O nome da categoria é obrigatório." });


            return CreatedAtAction(nameof(BuscarPorId), new { id = categoria.Id }, categoria);
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar([FromBody] Categoria categoria)
        {
            var sucesso = await _service.AtualizarAsync(categoria);
            if (!sucesso)
                return BadRequest(new { mensagem = "Dados inválidos ou categoria não encontrada." });

            return Ok(new { mensagem = "Categoria atualizada com sucesso." });
        }

        // RF04: Deleta a categoria caso não possua chamados atrelados
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var (sucesso, mensagem) = await _service.DeletarAsync(id);

            if (!sucesso)
            {
                if (mensagem == "Categoria não encontrada.")
                    return NotFound(new { mensagem });

                return BadRequest(new { mensagem });
            }

            return Ok(new { mensagem });
        }
    }
}