using MottoMap.Models;
using Microsoft.EntityFrameworkCore;

namespace MottoMap.Data.AppData
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<FuncionarioEntity> Funcionarios { get; set; }
        public DbSet<FilialEntity> Filiais { get; set; }
        public DbSet<MotosEntity> Motos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração das relações
            modelBuilder.Entity<FuncionarioEntity>()
                .HasOne(f => f.Filial)
                .WithMany(fl => fl.Funcionarios)
                .HasForeignKey(f => f.IdFilial)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MotosEntity>()
                .HasOne(m => m.Filial)
                .WithMany(fl => fl.Motos)
                .HasForeignKey(m => m.IdFilial)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurações específicas para Oracle
            modelBuilder.Entity<FuncionarioEntity>()
                .Property(f => f.IdFuncionario)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<FilialEntity>()
                .Property(f => f.IdFilial)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MotosEntity>()
                .Property(m => m.IdMoto)
                .ValueGeneratedOnAdd();

            // Índices para melhor performance
            modelBuilder.Entity<FuncionarioEntity>()
                .HasIndex(f => f.Email)
                .IsUnique();

            modelBuilder.Entity<MotosEntity>()
                .HasIndex(m => m.Placa)
                .IsUnique();
        }
    }
}
