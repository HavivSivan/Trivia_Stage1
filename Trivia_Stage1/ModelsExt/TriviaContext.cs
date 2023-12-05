using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia_Stage1.Models
{
    public partial class TriviaContext
    {
        public Player GetPlayerByEmail(string email)
        {
            return this.Players.Where(Player=>Player.Email==email).First();
        }
        public Question GetRandomQuestion()
        {
            Random rnd = new Random();
            int randomQuestion = rnd.Next(1, (this.Questions.Count() + 1));
            return this.Questions.Where(Question=>Question.QuestionId == rnd.Next(1, (this.Questions.Count()+1)));
        }
    }
}
