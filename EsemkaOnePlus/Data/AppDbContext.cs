using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EsemkaOnePlus.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Loyalty> Loyalties { get; set; } = null!;
        public virtual DbSet<Merchant> Merchants { get; set; } = null!;
        public virtual DbSet<Trade> Trades { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=SMKN1BANTUL;Initial Catalog=EsemkaOnePlus;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.PhotoPath).HasMaxLength(200);

                entity.HasOne(d => d.Loyalty)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.LoyaltyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customers_Tiers_LoyaltyId");
            });

            modelBuilder.Entity<Loyalty>(entity =>
            {
                entity.ToTable("Loyalty");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.RequiredPoint).HasColumnType("numeric(32, 9)");
            });

            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Location).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Trade>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Trades)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.Trades)
                    .HasForeignKey(d => d.VoucherId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Point).HasColumnType("numeric(32, 9)");

                entity.Property(e => e.Price).HasColumnType("numeric(32, 9)");

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Merchant)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.MerchantId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Cost).HasColumnType("numeric(32, 9)");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
