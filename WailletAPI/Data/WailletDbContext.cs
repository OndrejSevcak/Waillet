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

    public virtual DbSet<Ledger> Ledgers { get; set; }

    public virtual DbSet<SwapTransaction> SwapTransactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccKey).HasName("PK__Accounts__2D40EF1C1364D57D");

            entity.HasIndex(e => new { e.UserKey, e.Asset }, "UQ_User_Asset").IsUnique();

            entity.Property(e => e.Asset)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.UserKeyNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accounts__UserKe__3F466844");
        });

        modelBuilder.Entity<Ledger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ledger__3214EC07DC8A7D28");

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
                .HasConstraintName("FK__Ledger__AccKey__4AB81AF0");
        });

        modelBuilder.Entity<SwapTransaction>(entity =>
        {
            entity.HasKey(e => e.SwapId).HasName("PK__SwapTran__816B3609172FB65E");

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
                .HasConstraintName("FK__SwapTrans__UserK__4316F928");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserKey).HasName("PK__Users__296ADCF122BD6EEC");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053464F18E81").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<WithdrawalRequest>(entity =>
        {
            entity.HasKey(e => e.WithId).HasName("PK__Withdraw__E369696A7D6F78F3");

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
                .HasConstraintName("FK__Withdrawa__UserK__46E78A0C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
