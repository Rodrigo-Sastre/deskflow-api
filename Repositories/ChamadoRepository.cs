using DeskFlow.API.Data;
using DeskFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repositories
{
    public class ChamadoRepository
    {
        private readonly AppDbContext _context;

        public ChamadoRepository(AppDbContext context)
        {
            _context = context;
        }

        // RF12: Monta dinamicamente a consulta LINQ conforme os filtros informados pelo usuário
        public async Task<List<Chamado>> BuscarTodosAsync(Status? status = null, Prioridade? prioridade = null, int? categoriaId = null)
        {
            var query = _context.Chamados
                .Include(c => c.Categoria)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            if (prioridade.HasValue)
            {
                query = query.Where(c => c.Prioridade == prioridade.Value);
            }

            if (categoriaId.HasValue)
            {
                query = query.Where(c => c.CategoriaId == categoriaId.Value);
            }

            return await query.ToListAsync();
        }

        // RF11: Carrega os dados do chamado juntamente com sua Categoria e histórico de Interações
        public async Task<Chamado?> BuscarPorIdAsync(int id)
        {
            return await _context.Chamados
                .Include(c => c.Categoria)
                .Include(c => c.Interacoes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AdicionarAsync(Chamado chamado)
        {
            await _context.Chamados.AddAsync(chamado);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Chamado chamado)
        {
            _context.Chamados.Update(chamado);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Chamado chamado)
        {
            _context.Chamados.Remove(chamado);
            await _context.SaveChangesAsync();
        }
        public async Task AdicionarInteracaoAsync(Interacao interacao)
        {
            await _context.Interacoes.AddAsync(interacao);
            await _context.SaveChangesAsync();
        }
    }
}