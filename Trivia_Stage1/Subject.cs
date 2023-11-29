using System;
using System.Collections.Generic;

namespace Trivia_Stage1;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string? SubjectName { get; set; }

    public virtual ICollection<Question> Questions { get; } = new List<Question>();
}
