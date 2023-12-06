﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia_Stage1.Models
{
    public partial class TriviaContext
    {
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
            try
            {
                  return player.Rank.RankName;
            }
            catch
            {
                return null;
            }
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
        public void ChangePoints(Player p, bool b)
        {
            if (b) p.Points += 10;
            else p.Points -= 5;
            if (p.Points<0) p.Points = 0;
            SaveChanges();
        }
    }
}
