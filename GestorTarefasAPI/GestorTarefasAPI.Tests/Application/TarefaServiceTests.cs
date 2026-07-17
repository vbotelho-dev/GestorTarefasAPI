using Application.DTOs;
using Application.Services;
using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Core.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GestorTarefasAPI.Tests.Application;

public class TarefaServiceTests
{
    private readonly Mock<ITarefaRepository> _repositoryMock;
    private readonly TarefaService _service;

    public TarefaServiceTests()
    {
        _repositoryMock = new Mock<ITarefaRepository>();
        var loggerMock = new Mock<ILogger<TarefaService>>();
        _service = new TarefaService(_repositoryMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task CriarAsync_DeveCriarTarefaEChamarRepositorio_QuandoDadosValidos()
    {
        var dto = new CriarTarefaDto
        {
            Titulo = "Nova tarefa",
            Descricao = "Descricao",
            DataVencimento = DateTime.Now.AddDays(3)
        };

        var resultado = await _service.CriarAsync(dto);

        Assert.Equal(dto.Titulo, resultado.Titulo);
        Assert.Equal(Status.Pendente, resultado.Status);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Tarefa>()), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_DevePropagarDomainException_QuandoTituloForInvalido()
    {
        var dto = new CriarTarefaDto { Titulo = "", Descricao = "Descricao" };

        await Assert.ThrowsAsync<DomainException>(() => _service.CriarAsync(dto));

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Tarefa>()), Times.Never);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarTarefa_QuandoExiste()
    {
        var tarefa = new Tarefa("Titulo existente", "Descricao", null);
        _repositoryMock.Setup(r => r.GetByIdAsync(tarefa.Id)).ReturnsAsync(tarefa);

        var resultado = await _service.ObterPorIdAsync(tarefa.Id);

        Assert.NotNull(resultado);
        Assert.Equal(tarefa.Titulo, resultado!.Titulo);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        var idInexistente = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(idInexistente)).ReturnsAsync((Tarefa?)null);

        var resultado = await _service.ObterPorIdAsync(idInexistente);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarTodasAsTarefasMapeadas()
    {
        var tarefas = new List<Tarefa>
        {
            new("Tarefa 1", null, null),
            new("Tarefa 2", null, null)
        };
        _repositoryMock.Setup(r => r.GetByFilterAsync(null, null)).ReturnsAsync(tarefas);

        var resultado = await _service.ListarAsync(null, null);

        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarTarefa_QuandoExiste()
    {
        var tarefa = new Tarefa("Titulo original", "Descricao original", null);
        _repositoryMock.Setup(r => r.GetByIdAsync(tarefa.Id)).ReturnsAsync(tarefa);

        var dto = new AtualizarTarefaDto
        {
            Titulo = "Titulo atualizado",
            Descricao = "Nova descricao",
            Status = Status.Concluida
        };

        var resultado = await _service.AtualizarAsync(tarefa.Id, dto);

        Assert.NotNull(resultado);
        Assert.Equal("Titulo atualizado", resultado!.Titulo);
        Assert.Equal(Status.Concluida, resultado.Status);
        _repositoryMock.Verify(r => r.UpdateAsync(tarefa), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_DeveRetornarNull_QuandoTarefaNaoExiste()
    {
        var idInexistente = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(idInexistente)).ReturnsAsync((Tarefa?)null);

        var dto = new AtualizarTarefaDto { Titulo = "Qualquer", Status = Status.Pendente };

        var resultado = await _service.AtualizarAsync(idInexistente, dto);

        Assert.Null(resultado);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Tarefa>()), Times.Never);
    }

    [Fact]
    public async Task RemoverAsync_DeveRemoverERetornarTrue_QuandoTarefaExiste()
    {
        var tarefa = new Tarefa("Titulo qualquer", null, null);
        _repositoryMock.Setup(r => r.GetByIdAsync(tarefa.Id)).ReturnsAsync(tarefa);

        var resultado = await _service.RemoverAsync(tarefa.Id);

        Assert.True(resultado);
        _repositoryMock.Verify(r => r.RemoveAsync(tarefa), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_DeveRetornarFalse_QuandoTarefaNaoExiste()
    {
        var idInexistente = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(idInexistente)).ReturnsAsync((Tarefa?)null);

        var resultado = await _service.RemoverAsync(idInexistente);

        Assert.False(resultado);
        _repositoryMock.Verify(r => r.RemoveAsync(It.IsAny<Tarefa>()), Times.Never);
    }
}