using Core.Entities;
using Core.Enums;

namespace Application.DTOs;

public class TarefaResponseDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime? DataVencimento { get; set; }
    public Status Status { get; set; }

    public static TarefaResponseDto FromEntity(Tarefa tarefa)
    {
        return new TarefaResponseDto
        {
            Id = tarefa.Id,
            Titulo = tarefa.Titulo,
            Descricao = tarefa.Descricao,
            DataVencimento = tarefa.DataVencimento,
            Status = tarefa.Status
        };
    }
}