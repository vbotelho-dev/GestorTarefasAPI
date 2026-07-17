using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repository;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class TarefaRepository : ITarefaRepository
{
    private readonly TarefaDbContext _context;

    public TarefaRepository(TarefaDbContext context)
    {
        _context = context;
    }

    public async Task<Tarefa?> GetByIdAsync(Guid id)
    {
        return await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Tarefa>> GetAllAsync()
    {
        return await _context.Tarefas.ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetByFilterAsync(Status? status, DateTime? dataVencimento)
    {
        var query = _context.Tarefas.AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (dataVencimento.HasValue)
            query = query.Where(t => t.DataVencimento.HasValue &&
                                      t.DataVencimento.Value.Date == dataVencimento.Value.Date);

        return await query.ToListAsync();
    }

    public async Task AddAsync(Tarefa tarefa)
    {
        await _context.Tarefas.AddAsync(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tarefa tarefa)
    {
        _context.Tarefas.Update(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Tarefa tarefa)
    {
        _context.Tarefas.Remove(tarefa);
        await _context.SaveChangesAsync();
    }
}