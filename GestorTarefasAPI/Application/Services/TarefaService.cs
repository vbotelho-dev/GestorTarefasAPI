using Application.DTOs;
using Application.Interfaces.Services;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class TarefaService : ITarefaService
{
    private readonly ITarefaRepository _repository;
    private readonly ILogger<TarefaService> _logger;

    public TarefaService(ITarefaRepository repository, ILogger<TarefaService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<TarefaResponseDto> CriarAsync(CriarTarefaDto dto)
    {
        var tarefa = new Tarefa(dto.Titulo, dto.Descricao, dto.DataVencimento);
        await _repository.AddAsync(tarefa);

        _logger.LogInformation("Tarefa {Id} criada com sucesso.", tarefa.Id);

        return TarefaResponseDto.FromEntity(tarefa);
    }

    public async Task<TarefaResponseDto?> ObterPorIdAsync(Guid id)
    {
        var tarefa = await _repository.GetByIdAsync(id);
        return tarefa is null ? null : TarefaResponseDto.FromEntity(tarefa);
    }

    public async Task<IEnumerable<TarefaResponseDto>> ListarAsync(Status? status, DateTime? dataVencimento)
    {
        var tarefas = await _repository.GetByFilterAsync(status, dataVencimento);
        return tarefas.Select(TarefaResponseDto.FromEntity);
    }

    public async Task<TarefaResponseDto?> AtualizarAsync(Guid id, AtualizarTarefaDto dto)
    {
        var tarefa = await _repository.GetByIdAsync(id);
        if (tarefa is null)
        {
            _logger.LogWarning("Tentativa de atualizar tarefa inexistente: {Id}", id);
            return null;
        }

        tarefa.Atualizar(dto.Titulo, dto.Descricao, dto.DataVencimento, dto.Status);
        await _repository.UpdateAsync(tarefa);

        _logger.LogInformation("Tarefa {Id} atualizada com sucesso.", id);

        return TarefaResponseDto.FromEntity(tarefa);
    }

    public async Task<bool> RemoverAsync(Guid id)
    {
        var tarefa = await _repository.GetByIdAsync(id);
        if (tarefa is null)
        {
            _logger.LogWarning("Tentativa de remover tarefa inexistente: {Id}", id);
            return false;
        }

        await _repository.RemoveAsync(tarefa);
        _logger.LogInformation("Tarefa {Id} removida com sucesso.", id);

        return true;
    }
}