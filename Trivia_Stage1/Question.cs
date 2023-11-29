using System;
using System.Collections.Generic;

namespace Trivia_Stage1;

public partial class Question
{
    public int QuestionId { get; set; }

    public int? PlayerId { get; set; }

    public string? Correct { get; set; }

    public string? Incorrect1 { get; set; }

    public string? Incorrect2 { get; set; }

    public string? Incorrect3 { get; set; }

    public string? QuestionText { get; set; }

    public int? SubjectId { get; set; }

    public virtual Player? Player { get; set; }

    public virtual Subject? Subject { get; set; }
}
