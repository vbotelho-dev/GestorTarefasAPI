using Core.Entities;
using Core.Enums;

namespace Core.Interfaces.Repository;

public interface ITarefaRepository
{
    Task<Tarefa?> GetByIdAsync(Guid id);
    Task<IEnumerable<Tarefa>> GetAllAsync();
    Task<IEnumerable<Tarefa>> GetByFilterAsync(Status? status, DateTime? dataVencimento);
    Task AddAsync(Tarefa tarefa);
    void Update(Tarefa tarefa);
    void Remove(Tarefa tarefa);
}