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
        public virtual DbSet<FavoriteCompany> FavoriteCompanies { get; set; }
        public virtual DbSet<FavoriteCompanyBranch> FavoriteCompaniesBranches { get; set; }
        public virtual DbSet<FrequentlyVisitedCompany> FrequentlyVisitedCompanies { get; set; }
        public virtual DbSet<FrequentlyVisitedCompanyBranch> FrequentlyVisitedCompaniesBranches { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public virtual DbSet<UserCredit> UserCredits { get; set; }
        public virtual DbSet<BankCard> BankCards { get; set; }
        public virtual DbSet<UserRefillTransaction> UserRefillTransactions { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = Startup.Configuration.GetSection("Connection")["ConnectionString"].ToString();
                optionsBuilder.UseSqlServer(connectionString);                               
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


            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.PaymentTransactions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentTransactions_Users");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.PaymentTransactions)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentTransactions_Companies");


            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notifications_Users");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notifications_Companies");
            });

            modelBuilder.Entity<FavoriteCompanyBranch>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteCompaniesBranches)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteCompaniesBranches_Users");

                entity.HasOne(d => d.CompanyBranch)
                    .WithMany(p => p.FavoriteCompaniesBranches)
                    .HasForeignKey(d => d.CompanyBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteCompaniesBranches_CompanyBranches");
            });

            modelBuilder.Entity<FrequentlyVisitedCompanyBranch>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.FrequentlyVisitedCompaniesBranches)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FrequentlyVisitedCompaniesBranches_Users");

                entity.HasOne(d => d.CompanyBranch)
                    .WithMany(p => p.FrequentlyVisitedCompaniesBranches)
                    .HasForeignKey(d => d.CompanyBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FrequentlyVisitedCompaniesBranches_CompanyBranches");
            });

            modelBuilder.Entity<FrequentlyVisitedCompany>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.FrequentlyVisitedCompanies)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FrequentlyVisitedCompanies_Users");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.FrequentlyVisitedCompanies)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FrequentlyVisitedCompanies_Companies");
            });


            modelBuilder.Entity<FavoriteCompany>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteCompanies)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteCompanies_Users");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.FavoriteCompanies)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteCompanies_Companies");
            });

            modelBuilder.Entity<UserRefillTransaction>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRefillTransactions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRefillTransactions_Users");
            });

            modelBuilder.Entity<UserCredit>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserCredits)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCredits_Users");
            });

            modelBuilder.Entity<BankCard>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.BankCards)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankCards_Users");
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

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_UserCompanies");
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
