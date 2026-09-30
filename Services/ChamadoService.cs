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

        public async Task<(bool Sucesso, string Mensagem)> IniciarAtendimentoAsync(int id)
        {
            var chamado = await _repository.BuscarPorIdAsync(id);
            if (chamado == null)
                return (false, "Chamado não encontrado.");

            if (chamado.Status != Status.Aberto)
                return (false, "Apenas chamados com status 'Aberto' podem ser iniciados.");

            chamado.Status = Status.EmAndamento;
            await _repository.AtualizarAsync(chamado);

            return (true, "Atendimento iniciado com sucesso.");
        }


        public async Task<(bool Sucesso, string Mensagem)> EncerrarChamadoAsync(int id, string solucao)
        {
            if (string.IsNullOrWhiteSpace(solucao))
                return (false, "A solução é obrigatória para encerrar um chamado.");

            var chamado = await _repository.BuscarPorIdAsync(id);
            if (chamado == null)
                return (false, "Chamado não encontrado.");

            if (chamado.Status == Status.Fechado)
                return (false, "Este chamado já se encontra encerrado.");

            chamado.Status = Status.Fechado;
            chamado.DataFechamento = DateTime.UtcNow;
            chamado.Solucao = solucao;

            await _repository.AtualizarAsync(chamado);

            return (true, "Chamado encerrado com sucesso.");
        }
        public async Task<(bool Sucesso, string Mensagem)> AdicionarInteracaoAsync(int chamadoId, string autor, string mensagem)
        {
            if (string.IsNullOrWhiteSpace(autor) || string.IsNullOrWhiteSpace(mensagem))
                return (false, "Autor e Mensagem são obrigatórios.");

            var chamado = await _repository.BuscarPorIdAsync(chamadoId);
            if (chamado == null)
                return (false, "Chamado não encontrado.");

            if (chamado.Status == Status.Fechado)
                return (false, "Não é permitido adicionar interações em um chamado já encerrado.");

            var interacao = new Interacao
            {
                ChamadoId = chamadoId,
                QuemEscreveu = autor,
                Mensagem = mensagem,
                DataRegistro = DateTime.UtcNow
            };

            await _repository.AdicionarInteracaoAsync(interacao);
            return (true, "Interação adicionada com sucesso.");
        }
    }
}