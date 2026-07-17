using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Xunit;

namespace GestorTarefasAPI.Tests.Core;

public class TarefaTests
{
    [Fact]
    public void Construtor_DeveCriarTarefaComStatusPendente_QuandoDadosValidos()
    {
        var titulo = "Estudar para entrevista";
        var descricao = "Revisar DDD e SOLID";
        var dataVencimento = DateTime.Now.AddDays(5);

        var tarefa = new Tarefa(titulo, descricao, dataVencimento);

        Assert.NotEqual(Guid.Empty, tarefa.Id);
        Assert.Equal(titulo, tarefa.Titulo);
        Assert.Equal(descricao, tarefa.Descricao);
        Assert.Equal(dataVencimento, tarefa.DataVencimento);
        Assert.Equal(Status.Pendente, tarefa.Status);
    }

    [Fact]
    public void Construtor_DevePermitirDescricaoEDataNulas()
    {
        var tarefa = new Tarefa("Titulo qualquer", null, null);

        Assert.Null(tarefa.Descricao);
        Assert.Null(tarefa.DataVencimento);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Construtor_DeveLancarDomainException_QuandoTituloForInvalido(string? tituloInvalido)
    {
        var excecao = Assert.Throws<DomainException>(
            () => new Tarefa(tituloInvalido!, "descricao", null));

        Assert.Equal("O título da tarefa é obrigatório.", excecao.Message);
    }

    [Fact]
    public void Construtor_DeveLancarDomainException_QuandoDataVencimentoForNoPassado()
    {
        var dataPassada = DateTime.Now.AddDays(-1);

        var excecao = Assert.Throws<DomainException>(
            () => new Tarefa("Titulo", "descricao", dataPassada));

        Assert.Equal("A data de vencimento não pode estar no passado.", excecao.Message);
    }

    [Fact]
    public void Construtor_NaoDeveLancarExcecao_QuandoDataVencimentoForHoje()
    {
        var hoje = DateTime.Now.Date;

        var excecao = Record.Exception(() => new Tarefa("Titulo", "descricao", hoje));

        Assert.Null(excecao);
    }

    [Fact]
    public void Atualizar_DeveAtualizarTodosOsCampos_QuandoDadosValidos()
    {
        var tarefa = new Tarefa("Titulo original", "Descricao original", null);
        var novoTitulo = "Titulo atualizado";
        var novaDescricao = "Descricao atualizada";
        var novaData = DateTime.Now.AddDays(10);
        var novoStatus = Status.EmProgresso;

        tarefa.Atualizar(novoTitulo, novaDescricao, novaData, novoStatus);

        Assert.Equal(novoTitulo, tarefa.Titulo);
        Assert.Equal(novaDescricao, tarefa.Descricao);
        Assert.Equal(novaData, tarefa.DataVencimento);
        Assert.Equal(novoStatus, tarefa.Status);
    }

    [Fact]
    public void Atualizar_DeveLancarDomainException_QuandoNovoTituloForVazio()
    {
        var tarefa = new Tarefa("Titulo original", null, null);

        var excecao = Assert.Throws<DomainException>(
            () => tarefa.Atualizar("", "descricao", null, Status.Concluida));

        Assert.Equal("O título da tarefa é obrigatório.", excecao.Message);
    }

    [Fact]
    public void Atualizar_NaoDeveAlterarNada_QuandoLancaExcecao()
    {
        var tarefa = new Tarefa("Titulo original", "Descricao original", null);

        Assert.Throws<DomainException>(
            () => tarefa.Atualizar("", "nova descricao", null, Status.Concluida));

        Assert.Equal("Titulo original", tarefa.Titulo);
        Assert.Equal("Descricao original", tarefa.Descricao);
        Assert.Equal(Status.Pendente, tarefa.Status);
    }
}