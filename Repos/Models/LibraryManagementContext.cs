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

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookIssue> BookIssues { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<SubmitBooksInfo> SubmitBooksInfos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS22;Initial Catalog=LibraryManagement;User ID=sa;Password=Vedant@1997;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2AFBF18D814C");
        });

        modelBuilder.Entity<AuthenticationRefreshToken>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Authenti__CB9A1CFFE57B5817");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C2074B17AA90");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Category");
        });

        modelBuilder.Entity<BookIssue>(entity =>
        {
            entity.HasKey(e => new { e.IssueId, e.BookId, e.UserId }).HasName("PK__BookIssu__424F92E81CF2000B");

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
            entity.HasKey(e => new { e.CartId, e.BookId, e.UserId }).HasName("PK__Cart__7F75535B18CDFBF0");

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
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0BF7B89218");
        });

        modelBuilder.Entity<SubmitBooksInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubmitBo__3213E83F72A056E4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C0403B9C0");

            entity.Property(e => e.Role)
                .HasDefaultValueSql("('User')")
                .IsFixedLength();

            entity.HasOne(d => d.Address).WithMany(p => p.Users).HasConstraintName("FK_Users_Address");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
