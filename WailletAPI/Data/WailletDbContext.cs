using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WailletAPI.Entities;

namespace WailletAPI.Data;

public partial class WailletDbContext : DbContext
{
    public WailletDbContext()
    {
    }

    public WailletDbContext(DbContextOptions<WailletDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<Ledger> Ledgers { get; set; }

    public virtual DbSet<SwapTransaction> SwapTransactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WailletDb2;Trusted_Connection=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccKey).HasName("PK__Accounts__2D40EF1C288176A6");

            entity.HasIndex(e => new { e.UserKey, e.Asset }, "UQ_User_Asset").IsUnique();

            entity.Property(e => e.Asset)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.UserKeyNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accounts__UserKe__5441852A");
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Assets__3214EC0799039533");

            entity.HasIndex(e => e.Symbol, "UQ_SupportedAssets_Symbol").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Decimals).HasDefaultValue((byte)8);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Symbol).HasMaxLength(16);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Ledger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ledger__3214EC07C4F0F14A");

            entity.ToTable("Ledger");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 18)");
            entity.Property(e => e.Asset)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ReferenceType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.AccKeyNavigation).WithMany(p => p.Ledgers)
                .HasForeignKey(d => d.AccKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ledger__AccKey__5FB337D6");
        });

        modelBuilder.Entity<SwapTransaction>(entity =>
        {
            entity.HasKey(e => e.SwapId).HasName("PK__SwapTran__816B3609CD901E91");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.FeeAmount).HasColumnType("decimal(38, 18)");
            entity.Property(e => e.FromAmount).HasColumnType("decimal(38, 18)");
            entity.Property(e => e.FromAsset)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Rate).HasColumnType("decimal(38, 18)");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ToAmount).HasColumnType("decimal(38, 18)");
            entity.Property(e => e.ToAsset)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.UserKeyNavigation).WithMany(p => p.SwapTransactions)
                .HasForeignKey(d => d.UserKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SwapTrans__UserK__5812160E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserKey).HasName("PK__Users__296ADCF11662AC49");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534BBB61940").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<WithdrawalRequest>(entity =>
        {
            entity.HasKey(e => e.WithId).HasName("PK__Withdraw__E369696A0BCC9B97");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 18)");
            entity.Property(e => e.Asset)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.BlockchainTxId).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.FeeAmount).HasColumnType("decimal(38, 18)");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ToAddress).HasMaxLength(255);

            entity.HasOne(d => d.UserKeyNavigation).WithMany(p => p.WithdrawalRequests)
                .HasForeignKey(d => d.UserKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Withdrawa__UserK__5BE2A6F2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
