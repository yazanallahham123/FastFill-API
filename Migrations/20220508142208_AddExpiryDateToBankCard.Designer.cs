﻿// <auto-generated />
using System;
using FastFill_API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FastFill_API.Migrations
{
    [DbContext(typeof(FastFillDBContext))]
    [Migration("20220508142208_AddExpiryDateToBankCard")]
    partial class AddExpiryDateToBankCard
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Arabic_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FastFill_API.BankCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpiryDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BankCards");
                });

            modelBuilder.Entity("FastFill_API.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ArabicAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArabicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankAccountId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Disabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("EnglishAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnglishName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsOpen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("decimal(18,0)");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("LogoURL");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("decimal(18,0)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("FastFill_API.CompanyBranch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ArabicAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArabicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankAccountId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<bool?>("Disabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("EnglishAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnglishName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsDedicatedBankAccount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<bool?>("IsOpen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("decimal(18,0)");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("decimal(18,0)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("CompanyBranches");
                });

            modelBuilder.Entity("FastFill_API.ErrorLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("InnerMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ErrorLogs");
                });

            modelBuilder.Entity("FastFill_API.FavoriteCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UserId");

                    b.ToTable("FavoriteCompanies");
                });

            modelBuilder.Entity("FastFill_API.FavoriteCompanyBranch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyBranchId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyBranchId");

                    b.HasIndex("UserId");

                    b.ToTable("FavoriteCompaniesBranches");
                });

            modelBuilder.Entity("FastFill_API.FrequentlyVisitedCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("VisitDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UserId");

                    b.ToTable("FrequentlyVisitedCompanies");
                });

            modelBuilder.Entity("FastFill_API.FrequentlyVisitedCompanyBranch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyBranchId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("VisitDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CompanyBranchId");

                    b.HasIndex("UserId");

                    b.ToTable("FrequentlyVisitedCompaniesBranches");
                });

            modelBuilder.Entity("FastFill_API.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Cleared")
                        .HasColumnType("bit");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Liters")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Material")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Price")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("FastFill_API.PaymentTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int?>("CompanyBranchId")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Fastfill")
                        .HasColumnType("float");

                    b.Property<int>("FuelTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyBranchId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UserId");

                    b.ToTable("PaymentTransactions");
                });

            modelBuilder.Entity("FastFill_API.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LongId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("TransactionReferenceId")
                        .HasColumnType("int");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.Property<decimal>("TransactionValue")
                        .HasColumnType("decimal(18,0)");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.Property<Guid>("WalletLongId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TransactionType");

                    b.HasIndex("WalletId");

                    b.HasIndex("WalletLongId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("FastFill_API.TransactionType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("ArabicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TransactionTypes");
                });

            modelBuilder.Entity("FastFill_API.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<bool?>("Disabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("FirebaseToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Language")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<byte[]>("StoredSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Token")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FastFill_API.UserCredit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserCredits");
                });

            modelBuilder.Entity("FastFill_API.UserRefillTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.Property<string>("transactionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRefillTransactions");
                });

            modelBuilder.Entity("FastFill_API.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("ArabicName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("FastFill_API.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LoginKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LongId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "LongId" }, "IX_Wallets_LongId_Unqiue")
                        .IsUnique();

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("FastFill_API.BankCard", b =>
                {
                    b.HasOne("FastFill_API.User", "User")
                        .WithMany("BankCards")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_BankCards_Users")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FastFill_API.CompanyBranch", b =>
                {
                    b.HasOne("FastFill_API.Company", "Company")
                        .WithMany("CompanyBranches")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK_CompanyBranches_Companies")
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("FastFill_API.FavoriteCompany", b =>
                {
                    b.HasOne("FastFill_API.Company", "Company")
                        .WithMany("FavoriteCompanies")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK_FavoriteCompanies_Companies")
                        .IsRequired();

                    b.HasOne("FastFill_API.User", "User")
                        .WithMany("FavoriteCompanies")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_FavoriteCompanies_Users")
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FastFill_API.FavoriteCompanyBranch", b =>
                {
                    b.HasOne("FastFill_API.CompanyBranch", "CompanyBranch")
                        .WithMany("FavoriteCompaniesBranches")
                        .HasForeignKey("CompanyBranchId")
                        .HasConstraintName("FK_FavoriteCompaniesBranches_CompanyBranches")
                        .IsRequired();

                    b.HasOne("FastFill_API.User", "User")
                        .WithMany("FavoriteCompaniesBranches")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_FavoriteCompaniesBranches_Users")
                        .IsRequired();

                    b.Navigation("CompanyBranch");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FastFill_API.FrequentlyVisitedCompany", b =>
                {
                    b.HasOne("FastFill_API.Company", "Company")
                        .WithMany("FrequentlyVisitedCompanies")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK_FrequentlyVisitedCompanies_Companies")
                        .IsRequired();

                    b.HasOne("FastFill_API.User", "User")
                        .WithMany("FrequentlyVisitedCompanies")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_FrequentlyVisitedCompanies_Users")
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FastFill_API.FrequentlyVisitedCompanyBranch", b =>
                {
                    b.HasOne("FastFill_API.CompanyBranch", "CompanyBranch")
                        .WithMany("FrequentlyVisitedCompaniesBranches")
                        .HasForeignKey("CompanyBranchId")
                        .HasConstraintName("FK_FrequentlyVisitedCompaniesBranches_CompanyBranches")
                        .IsRequired();

                    b.HasOne("FastFill_API.User", "User")
                        .WithMany("FrequentlyVisitedCompaniesBranches")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_FrequentlyVisitedCompaniesBranches_Users")
                        .IsRequired();

                    b.Navigation("CompanyBranch");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FastFill_API.Notification", b =>
                {
                    b.HasOne("FastFill_API.Company", "Company")
                        .WithMany("Notifications")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK_Notifications_Companies");

                    b.HasOne("FastFill_API.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Notifications_Users");

                    b.Navigation("Company");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FastFill_API.PaymentTransaction", b =>
                {
                    b.HasOne("FastFill_API.CompanyBranch", null)
                        .WithMany("PaymentTransactions")
                        .HasForeignKey("CompanyBranchId");

                    b.HasOne("FastFill_API.Company", "Company")
                        .WithMany("PaymentTransactions")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK_PaymentTransactions_Companies")
                        .IsRequired();

                    b.HasOne("FastFill_API.User", "User")
                        .WithMany("PaymentTransactions")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_PaymentTransactions_Users")
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FastFill_API.Transaction", b =>
                {
                    b.HasOne("FastFill_API.TransactionType", "TransactionTypeNavigation")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionType")
                        .HasConstraintName("FK_Transactions_TransactionTypes")
                        .IsRequired();

                    b.HasOne("FastFill_API.Wallet", "Wallet")
                        .WithMany("TransactionWallets")
                        .HasForeignKey("WalletId")
                        .HasConstraintName("FK_Transactions_Wallets")
                        .IsRequired();

                    b.HasOne("FastFill_API.Wallet", "WalletLong")
                        .WithMany("TransactionWalletLongs")
                        .HasForeignKey("WalletLongId")
                        .HasConstraintName("FK_Transactions_Wallets_LongId")
                        .HasPrincipalKey("LongId")
                        .IsRequired();

                    b.Navigation("TransactionTypeNavigation");

                    b.Navigation("Wallet");

                    b.Navigation("WalletLong");
                });

            modelBuilder.Entity("FastFill_API.User", b =>
                {
                    b.HasOne("FastFill_API.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK_Users_UserCompanies");

                    b.HasOne("FastFill_API.UserRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_Users_UserRoles")
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("FastFill_API.UserCredit", b =>
                {
                    b.HasOne("FastFill_API.User", "User")
                        .WithMany("UserCredits")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserCredits_Users")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FastFill_API.UserRefillTransaction", b =>
                {
                    b.HasOne("FastFill_API.User", "User")
                        .WithMany("UserRefillTransactions")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserRefillTransactions_Users")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FastFill_API.Wallet", b =>
                {
                    b.HasOne("FastFill_API.User", "User")
                        .WithMany("Wallets")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Wallets_Users")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FastFill_API.Company", b =>
                {
                    b.Navigation("CompanyBranches");

                    b.Navigation("FavoriteCompanies");

                    b.Navigation("FrequentlyVisitedCompanies");

                    b.Navigation("Notifications");

                    b.Navigation("PaymentTransactions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("FastFill_API.CompanyBranch", b =>
                {
                    b.Navigation("FavoriteCompaniesBranches");

                    b.Navigation("FrequentlyVisitedCompaniesBranches");

                    b.Navigation("PaymentTransactions");
                });

            modelBuilder.Entity("FastFill_API.TransactionType", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("FastFill_API.User", b =>
                {
                    b.Navigation("BankCards");

                    b.Navigation("FavoriteCompanies");

                    b.Navigation("FavoriteCompaniesBranches");

                    b.Navigation("FrequentlyVisitedCompanies");

                    b.Navigation("FrequentlyVisitedCompaniesBranches");

                    b.Navigation("Notifications");

                    b.Navigation("PaymentTransactions");

                    b.Navigation("UserCredits");

                    b.Navigation("UserRefillTransactions");

                    b.Navigation("Wallets");
                });

            modelBuilder.Entity("FastFill_API.UserRole", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("FastFill_API.Wallet", b =>
                {
                    b.Navigation("TransactionWalletLongs");

                    b.Navigation("TransactionWallets");
                });
#pragma warning restore 612, 618
        }
    }
}
