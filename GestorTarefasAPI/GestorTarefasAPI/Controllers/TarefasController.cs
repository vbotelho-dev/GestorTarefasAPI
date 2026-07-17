using Application.DTOs;
using Application.Interfaces.Services;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GestorTarefasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TarefasController : ControllerBase
{
    private readonly ITarefaService _tarefaService;
    private readonly ILogger<TarefasController> _logger;

    public TarefasController(ITarefaService tarefaService, ILogger<TarefasController> logger)
    {
        _tarefaService = tarefaService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova tarefa.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TarefaResponseDto>> Criar([FromBody] CriarTarefaDto dto)
    {
        var tarefa = await _tarefaService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
    }

    /// <summary>
    /// Lista todas as tarefas, com filtro opcional por status e/ou data de vencimento.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TarefaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TarefaResponseDto>>> Listar(
        [FromQuery] Status? status,
        [FromQuery] DateTime? dataVencimento)
    {
        var tarefas = await _tarefaService.ListarAsync(status, dataVencimento);
        return Ok(tarefas);
    }

    /// <summary>
    /// Obtém uma tarefa pelo seu identificador.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TarefaResponseDto>> ObterPorId(Guid id)
    {
        var tarefa = await _tarefaService.ObterPorIdAsync(id);

        if (tarefa is null)
        {
            _logger.LogWarning("Tarefa {Id} não encontrada.", id);
            return NotFound(new { mensagem = $"Tarefa com id '{id}' não foi encontrada." });
        }

        return Ok(tarefa);
    }

    /// <summary>
    /// Atualiza uma tarefa existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TarefaResponseDto>> Atualizar(Guid id, [FromBody] AtualizarTarefaDto dto)
    {
        var tarefa = await _tarefaService.AtualizarAsync(id, dto);

        if (tarefa is null)
        {
            _logger.LogWarning("Tentativa de atualizar tarefa inexistente: {Id}", id);
            return NotFound(new { mensagem = $"Tarefa com id '{id}' não foi encontrada." });
        }

        return Ok(tarefa);
    }

    /// <summary>
    /// Remove uma tarefa pelo seu identificador.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(Guid id)
    {
        var removido = await _tarefaService.RemoverAsync(id);

        if (!removido)
        {
            _logger.LogWarning("Tentativa de remover tarefa inexistente: {Id}", id);
            return NotFound(new { mensagem = $"Tarefa com id '{id}' não foi encontrada." });
        }

        return NoContent();
    }
}