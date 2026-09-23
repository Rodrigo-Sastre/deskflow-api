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

        public async Task<List<Chamado>> BuscarTodosAsync()
        {

            return await _context.Chamados
                .Include(c => c.Categoria)
                .ToListAsync();
        }

        public async Task<Chamado?> BuscarPorIdAsync(int id)
        {
            return await _context.Chamados
                .Include(c => c.Categoria)
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
    }
}