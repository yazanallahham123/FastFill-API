using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FastFill_API
{
    public partial class FastFillDBContext : DbContext
    {
        public FastFillDBContext()
        {
        }

        public FastFillDBContext(DbContextOptions<FastFillDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyBranch> CompanyBranches { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Data Source=localhost,8765;Initial Catalog=CloiDB;Persist Security Info=True;User ID=sa;Password=Techno$o;
                optionsBuilder.UseSqlServer("Server=192.99.16.179,8765;Database=FastFill;User Id=sa;Password=Techno$o;");
                //optionsBuilder.UseSqlServer("Server=.\\;Database=FastFillDB;User ID=sa;Password=Techno$o;");                               
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_CI_AS");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.ArabicName).IsRequired();

                entity.Property(e => e.Disabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsOpen).HasDefaultValueSql("((1))");

                entity.Property(e => e.Latitude).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.LogoUrl).HasColumnName("LogoURL");

                entity.Property(e => e.Longitude).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<CompanyBranch>(entity =>
            {
                entity.Property(e => e.ArabicName).IsRequired();

                entity.Property(e => e.Disabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDedicatedBankAccount).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsOpen).HasDefaultValueSql("((1))");

                entity.Property(e => e.Latitude).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyBranches)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyBranches_Companies");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.LongId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionValue).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.TransactionTypeNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TransactionType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_TransactionTypes");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.TransactionWallets)
                    .HasForeignKey(d => d.WalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Wallets");

                entity.HasOne(d => d.WalletLong)
                    .WithMany(p => p.TransactionWalletLongs)
                    .HasPrincipalKey(p => p.LongId)
                    .HasForeignKey(d => d.WalletLongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Wallets_LongId");
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ArabicName).IsRequired();

                entity.Property(e => e.EnglishName).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Disabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Token).HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_UserRoles");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ArabicName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasIndex(e => e.LongId, "IX_Wallets_LongId_Unqiue")
                    .IsUnique();

                entity.Property(e => e.LoginKey).IsRequired();

                entity.Property(e => e.LongId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wallets_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
