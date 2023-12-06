using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia_Stage1.Models
{
    public partial class TriviaContext
    {
        public string GetRankByPlayer(Player player)
        {
            return this.Ranks.Where(x =>player.Ranking==x.RankId).FirstOrDefault();
        }
        public Player GetPlayerByEmail(string email)
        {
            try
            {
                 return this.Players.Where(Player=>Player.Email==email).First();
            }
            catch
            {
                return null;
            }
        }
        public Player SignUp(string username,string password,string email)
        {
            if(GetPlayerByEmail(email)!= null)
            {
                return null;
            }
            Player p = new Player() {Email=email,PlayerName=username,Password=password,Ranking=1,Points=0,QuestionsMade=0 };
            try
            {
                this.Players.Add(p);
                SaveChanges();
                return p;
            }
            catch
            {
                return null;
            }
            return null;
        }
        public Question GetRandomQuestion()
        {
            Random rnd = new Random();
            int questionId = rnd.Next(1, (this.Questions.Count() + 1));
            return this.Questions.Where(Question => Question.QuestionId == questionId).Include(q=>q.Subject).FirstOrDefault();
        }
    }
}
