using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

    public class TarefaDbContext : DbContext
    {
        public TarefaDbContext(DbContextOptions<TarefaDbContext> options) : base(options)
        {
        }

        public DbSet<Tarefa> Tarefas => Set<Tarefa>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Titulo)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(t => t.Descricao)
                    .HasMaxLength(1000);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
