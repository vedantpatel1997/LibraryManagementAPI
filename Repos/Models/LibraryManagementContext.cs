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

    public virtual DbSet<AuthenticationRefreshToken> AuthenticationRefreshTokens { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookIssue> BookIssues { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS22;Initial Catalog=LibraryManagement;User ID=sa;Password=Vedant@1997;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthenticationRefreshToken>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Authenti__CB9A1CFF14845236");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C2074923FFCD");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Books__CategoryI__4222D4EF");
        });

        modelBuilder.Entity<BookIssue>(entity =>
        {
            entity.HasKey(e => new { e.IssueId, e.BookId, e.UserId }).HasName("PK__BookIssu__424F92E89BC99445");

            entity.Property(e => e.IssueId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Book).WithMany(p => p.BookIssues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookIssue_BookId");

            entity.HasOne(d => d.User).WithMany(p => p.BookIssues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookIssue_UserId");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B1C89C560");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB4D2A9CA");

            entity.Property(e => e.Role)
                .HasDefaultValueSql("('User')")
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
