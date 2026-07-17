using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Application.DTOs;

public class AtualizarTarefaDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
    public string? Descricao { get; set; }

    public DateTime? DataVencimento { get; set; }

    [Required(ErrorMessage = "O status é obrigatório.")]
    public Status Status { get; set; }
}