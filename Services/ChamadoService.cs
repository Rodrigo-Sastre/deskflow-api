using DeskFlow.API.Models;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services
{
    public class ChamadoService(ChamadoRepository repository, CategoriaRepository categoriaRepository)
    {
        private readonly ChamadoRepository _repository = repository;
        private readonly CategoriaRepository _categoriaRepository = categoriaRepository;

        public async Task<List<Chamado>> BuscarTodosAsync()
        {
            return await _repository.BuscarTodosAsync();
        }

        public async Task<Chamado?> BuscarPorIdAsync(int id)
        {
            return await _repository.BuscarPorIdAsync(id);
        }

        public async Task<bool> AdicionarAsync(Chamado chamado)
        {

            if (string.IsNullOrWhiteSpace(chamado.SolicitanteNome) || string.IsNullOrWhiteSpace(chamado.Titulo))
                return false;

            var categoriaExiste = await _categoriaRepository.BuscarPorIdAsync(chamado.CategoriaId);
            if (categoriaExiste == null)
                return false;


            if (categoriaExiste == null)
                return false;


            chamado.Status = Status.Aberto;
            chamado.DataAbertura = DateTime.UtcNow;

            await _repository.AdicionarAsync(chamado);
            return true;
        }

        public async Task<bool> AtualizarAsync(Chamado chamado)
        {
            var chamadoExistente = await _repository.BuscarPorIdAsync(chamado.Id);
            if (chamadoExistente == null)
                return false;


            chamadoExistente.Titulo = chamado.Titulo;
            chamadoExistente.Descricao = chamado.Descricao;
            chamadoExistente.Prioridade = chamado.Prioridade;


            chamadoExistente.Status = chamado.Status;

            await _repository.AtualizarAsync(chamadoExistente);
            return true;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var chamado = await _repository.BuscarPorIdAsync(id);
            if (chamado == null)
                return false;

            await _repository.DeletarAsync(chamado);
            return true;
        }
    }
}