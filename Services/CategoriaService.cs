using DeskFlow.API.Models;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services
{
    public class CategoriaService
    {
        private readonly CategoriaRepository _repository;


        public CategoriaService(CategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Categoria>> BuscarTodasAsync()
        {
            return await _repository.BuscarTodasAsync();
        }

        public async Task<Categoria?> BuscarPorIdAsync(int id)
        {
            return await _repository.BuscarPorIdAsync(id);
        }

        public async Task<bool> AdicionarAsync(Categoria categoria)
        {

            if (string.IsNullOrWhiteSpace(categoria.Nome))
                return false;

            await _repository.AdicionarAsync(categoria);
            return true;
        }

        public async Task<bool> AtualizarAsync(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nome))
                return false;

            var categoriaExistente = await _repository.BuscarPorIdAsync(categoria.Id);
            if (categoriaExistente == null)
                return false;


            categoriaExistente.Nome = categoria.Nome;
            categoriaExistente.Ativo = categoria.Ativo;

            await _repository.AtualizarAsync(categoriaExistente);
            return true;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var categoria = await _repository.BuscarPorIdAsync(id);
            if (categoria == null)
                return false;

            await _repository.DeletarAsync(categoria);
            return true;
        }
    }
}
