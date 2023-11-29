using System;
using System.Collections.Generic;

namespace Trivia_Stage1;

public partial class Player
{
    public int PlayerId { get; set; }

    public string? Email { get; set; }

    public string? PlayerName { get; set; }

    public int? Ranking { get; set; }

    public int? Points { get; set; }

    public int? QuestionsMade { get; set; }

    public virtual ICollection<Question> Questions { get; } = new List<Question>();
}
