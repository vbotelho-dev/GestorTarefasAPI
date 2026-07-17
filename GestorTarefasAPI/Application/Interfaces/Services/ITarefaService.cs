using Application.DTOs;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface ITarefaService
    {
        Task<TarefaResponseDto> CriarAsync(CriarTarefaDto dto);
        Task<TarefaResponseDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<TarefaResponseDto>> ListarAsync(Status? status, DateTime? dataVencimento);
        Task<TarefaResponseDto?> AtualizarAsync(Guid id, AtualizarTarefaDto dto);
        Task<bool> RemoverAsync(Guid id);
    }
}
