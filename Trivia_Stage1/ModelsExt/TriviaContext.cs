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
        public bool ChangeEmail(Player player,string email_)
        {
            if (GetPlayerByEmail(email_) != null)
            {
                Console.WriteLine("Player with this email already exists");
                return false;
            }
            try
            {
                player.Email = email_;
                SaveChanges();
                return true;
            }
            catch
            {
                Console.WriteLine("Something went wrng while changing the mail");
                return false;
            }
        }
        public bool ChangeName(Player player,string Name_)
        {
            try
            {
                player.PlayerName = Name_;
                SaveChanges();
                return true;
            }
            catch
            {
                Console.WriteLine("Something went wrng while changing the name");
                return false;
            }
        }
        public bool ChangePassword(Player player,string Password_)
        {
            try
            {
                player.Password = Password_;
                SaveChanges();
                return true;
            }
            catch
            {
                Console.WriteLine("Something went wrong while changing password");
                return false;
            }
        }
        public void AddQuestion(string text_, string correct_, string wrong1_, string wrong2_, string wrong3_, int PlayerId_, int subject_)
        {
            try
            {
                Question question = new Question() { PlayerId=PlayerId_, Correct=correct_, Incorrect1=wrong1_, Incorrect2 = wrong2_, Incorrect3 = wrong3_, QuestionText=text_, SubjectId=subject_, StatusId=1, };
                this.Questions.Add(question);
                SaveChanges();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); Console.WriteLine("Failed, please try again."); }
        }
        public string GetRankByPlayer(Player player)
        {
            return player.Rank.RankName;
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
            return this.Questions.Where(Question => Question.QuestionId == questionId/* && Question.StatusId == 2*/).Include(q=>q.Subject).FirstOrDefault();
        }
        public Question GetPendingQuestion()
        {
            return this.Questions.Where(Question => Question.StatusId == 1).FirstOrDefault();
        }
        public void ChangePoints(Player p, bool b)
        {
            if (b) p.Points += 10;
            else p.Points -= 5;
            if (p.Points<0) p.Points = 0;
            SaveChanges();
        }
    }
}
