﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trivia_Stage1.Models;

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
        => optionsBuilder.UseSqlServer("Server = DESKTOP-JI7C9H0; Database=Trivia;\nTrusted_Connection=true; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PK__Players__4A4E74C875ACD127");

            entity.HasOne(d => d.RankingNavigation).WithMany(p => p.Players).HasConstraintName("FK_Ranking");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FAC280CC20E");

            entity.HasOne(d => d.Player).WithMany(p => p.Questions).HasConstraintName("FK_PlayerId");

            entity.HasOne(d => d.Status).WithMany(p => p.Questions).HasConstraintName("FK_StatusId");

            entity.HasOne(d => d.Subject).WithMany(p => p.Questions).HasConstraintName("FK_SubjectId");
        });

        modelBuilder.Entity<QuestionStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Question__C8EE2063F38A5E17");
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.RankId).HasName("PK__Rank__B37AF876C57A6C9D");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subjects__AC1BA3A8B34ED62E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
