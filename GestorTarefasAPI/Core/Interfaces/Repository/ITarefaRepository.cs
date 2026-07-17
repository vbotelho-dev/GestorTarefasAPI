using Core.Entities;
using Core.Enums;

namespace Core.Interfaces.Repository;

public interface ITarefaRepository
{
    Task<Tarefa?> GetByIdAsync(Guid id);
    Task<IEnumerable<Tarefa>> GetAllAsync();
    Task<IEnumerable<Tarefa>> GetByFilterAsync(Status? status, DateTime? dataVencimento);
    Task AddAsync(Tarefa tarefa);
    Task UpdateAsync(Tarefa tarefa);
    Task RemoveAsync(Tarefa tarefa);
}