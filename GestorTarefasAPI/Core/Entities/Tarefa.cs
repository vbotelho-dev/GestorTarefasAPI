using Core.Enums;
using Core.Exceptions;

namespace Core.Entities;

public class Tarefa
{
    public Guid Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public DateTime? DataVencimento { get; private set; }
    public Status Status { get; private set; }

    private Tarefa() { }

    public Tarefa(string titulo, string? descricao, DateTime? dataVencimento)
    {
        ValidarTitulo(titulo);
        ValidarDataVencimento(dataVencimento);

        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        DataVencimento = dataVencimento;
        Status = Status.Pendente;
    }

    public void Atualizar(string titulo, string? descricao, DateTime? dataVencimento, Status status)
    {
        ValidarTitulo(titulo);

        Titulo = titulo;
        Descricao = descricao;
        DataVencimento = dataVencimento;
        Status = status;
    }

    private static void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("O título da tarefa é obrigatório.");
    }

    private static void ValidarDataVencimento(DateTime? dataVencimento)
    {
        if (dataVencimento.HasValue && dataVencimento.Value.Date < DateTime.Now.Date)
            throw new DomainException("A data de vencimento não pode estar no passado.");
    }
}