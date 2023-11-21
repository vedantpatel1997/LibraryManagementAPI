using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repos.Models;

public partial class LibraryManagementContext : DbContext
{
    public LibraryManagementContext()
    {
    }

    public LibraryManagementContext(DbContextOptions<LibraryManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AuthenticationRefreshToken> AuthenticationRefreshTokens { get; set; }

    public virtual DbSet<BillingBooksInfo> BillingBooksInfos { get; set; }

    public virtual DbSet<BillingSummary> BillingSummaries { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookIssue> BookIssues { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<SubmitBooksInfo> SubmitBooksInfos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2AFB0165DA66");
        });

        modelBuilder.Entity<AuthenticationRefreshToken>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Authenti__CB9A1CFF5D887F14");
        });

        modelBuilder.Entity<BillingBooksInfo>(entity =>
        {
            entity.HasKey(e => e.BillingBookInfoId).HasName("PK__BillingB__10F5F5FDF21CA7C4");

            entity.Property(e => e.BookCategory).IsFixedLength();

            entity.HasOne(d => d.Billing).WithMany(p => p.BillingBooksInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BillingBo__billi__693CA210");
        });

        modelBuilder.Entity<BillingSummary>(entity =>
        {
            entity.HasKey(e => e.BillingId).HasName("PK__BillingS__39667D67391D3D5A");

            entity.Property(e => e.Delivery).HasDefaultValueSql("((0))");
            entity.Property(e => e.Pickup).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Address).WithMany(p => p.BillingSummaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BillingSu__addre__6A30C649");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C2072DD4FCD8");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Category");
        });

        modelBuilder.Entity<BookIssue>(entity =>
        {
            entity.HasKey(e => new { e.IssueId, e.BookId, e.UserId }).HasName("PK__BookIssu__424F92E8AF38B2E7");

            entity.Property(e => e.IssueId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Book).WithMany(p => p.BookIssues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookIssue__BookI__47DBAE45");

            entity.HasOne(d => d.User).WithMany(p => p.BookIssues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookIssue__UserI__48CFD27E");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.BookId, e.UserId }).HasName("PK__Cart__7F75535B4B1BBC30");

            entity.Property(e => e.CartId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Book).WithMany(p => p.Carts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__BookId__4BAC3F29");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__UserId__4CA06362");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B66C059B6");
        });

        modelBuilder.Entity<SubmitBooksInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubmitBo__3213E83F99CBACC4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C17CE30C0");

            entity.Property(e => e.Role)
                .HasDefaultValueSql("('User')")
                .IsFixedLength();

            entity.HasOne(d => d.Address).WithMany(p => p.Users).HasConstraintName("FK_Users_Address");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
