using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trivia_Stage1;

public partial class TriviaContext : DbContext
{
    public TriviaContext()
    {
    }

    public TriviaContext(DbContextOptions<TriviaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionStatus> QuestionStatuses { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = localhost\\SQLEXPRESS; Database=Trivia;\nTrusted_Connection=true; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PK__Players__4A4E74C8F86C5E7F");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PlayerName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FACE1AB5F52");

            entity.Property(e => e.Correct)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Incorrect1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Incorrect2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Incorrect3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.QuestionText)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Player).WithMany(p => p.Questions)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("FK_PlayerId");

            entity.HasOne(d => d.Subject).WithMany(p => p.Questions)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_SubjectId");
        });

        modelBuilder.Entity<QuestionStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Question__C8EE206392FAD427");

            entity.ToTable("QuestionStatus");

            entity.Property(e => e.Status)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.RankId).HasName("PK__Rank__B37AF876F9A0A8E3");

            entity.ToTable("Rank");

            entity.Property(e => e.RankName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subjects__AC1BA3A80AB6BCC7");

            entity.Property(e => e.SubjectName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
