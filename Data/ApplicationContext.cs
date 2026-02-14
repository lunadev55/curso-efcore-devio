using Microsoft.EntityFrameworkCore;
using CursoEFCore.Domain;

namespace CursoEFCore.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<PedidoItem> PedidoItens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
        .UseSqlServer("Server=127.0.0.1, 14033\\sql1;Database=CursoEFCore;User Id=sa;Password=An,50}.!-@(.$]9-@;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(p =>
        {
            p.ToTable("Clientes"); 

            p.HasKey(p => p.Id);

            p.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired();
            p.Property(p => p.Telefone).HasColumnType("CHAR(11)");
            p.Property(p => p.CEP).HasColumnType("CHAR(8)").IsRequired();
            p.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
            p.Property(p => p.Cidade).HasMaxLength(60).IsRequired();

            p.HasIndex(i => i.Telefone).HasDatabaseName("idx_cliente_telefone");
        });

        modelBuilder.Entity<Produto>(p =>
        {
            p.ToTable("Produtos");

            p.HasKey(p => p.Id);

            p.Property(p => p.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
            p.Property(p => p.Descricao).HasColumnType("VARCHAR(60)");
            p.Property(p => p.Valor).IsRequired();
            p.Property(p => p.TipoProduto).HasConversion<string>();
        });

        modelBuilder.Entity<Pedido>(p =>
        {
            p.ToTable("Pedidos");

            p.HasKey(p => p.Id);

            p.Property(p => p.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            p.Property(p => p.Status).HasConversion<string>();
            p.Property(p => p.TipoFrete).HasConversion<int>();
            p.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

            p.HasMany(p => p.Itens)
                .WithOne(p => p.Pedido)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PedidoItem>(p =>
        {
            p.ToTable("PedidoItens");

            p.HasKey(p => p.Id);

            p.Property(p => p.Quantidade).HasDefaultValue(1).IsRequired();
            p.Property(p => p.Valor).IsRequired();
            p.Property(p => p.Desconto).IsRequired();
        });
    }
}