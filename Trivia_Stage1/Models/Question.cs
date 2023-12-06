using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Trivia_Stage1.Models;

public partial class Question
{
    [Key]
    public int QuestionId { get; set; }

    public int? PlayerId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Correct { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Incorrect1 { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Incorrect2 { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Incorrect3 { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? QuestionText { get; set; }

    public int? SubjectId { get; set; }

    public int? StatusId { get; set; }

    [ForeignKey("PlayerId")]
    [InverseProperty("Questions")]
    public virtual Player? Player { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Questions")]
    public virtual QuestionStatus? QuestionStatus { get; set; }

    [ForeignKey("SubjectId")]
    [InverseProperty("Questions")]
    public virtual Subject? Subject { get; set; }
    public Question(string text, string correct, string wrong1, string wrong2, string wrong3, int PlayerId, int subject)
    {
        this.QuestionText=text; this.Correct = correct; this.Incorrect1 = wrong1; this.Incorrect2= wrong2; this.Incorrect3= wrong3; this.SubjectId = subject;
    }
}
