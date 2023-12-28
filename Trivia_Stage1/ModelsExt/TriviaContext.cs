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
        /// <summary>
        /// Gets player details from the databases by the player id
        /// </summary>
        /// <param name="id">player id</param>
        /// <returns>player details from the specified id</returns>
        public Player GetPlayerByIdWithDetails(int playerid)
        {
            try
            {
                return this.Players.Where(p => p.PlayerId == playerid).Include(p => p.Questions).Include(p => p.Rank).FirstOrDefault();
            }
            catch 
            {
                return null;
            }

        }

        /// <summary>
        /// Advances specified player's rank
        /// </summary>
        /// <param name="Player">specified player</param>
        public void AdvanceRank(Player Player)
        {
            if(Player.RankId<2)Player.RankId++;
            SaveChanges();
        }
        /// <summary>
        /// Finds the amount of questions a player made
        /// </summary>
        /// <param name="player">requested player</param>
        /// <returns>number of questions requested player made</returns>
        public int QuestionsMadeByPlayer(Player player)
        {
            try
            {
                return this.Questions.Where(x  => x.Player == player).Count();
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// Changes the requested player's email
        /// </summary>
        /// <param name="player">The email of the player's email you want to changed</param>
        /// <param name="email_">The email you want to change to</param>
        /// <returns>false if email already exists or changing the email went worng. true if the email was updated successfully</returns>
        public bool ChangeEmail(Player player,string email_)
        {
            if (GetPlayerByEmail(email_) != null)//checks if the mail already exists by using an earlier method
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
        /// <summary>
        /// Changes the requested player's name
        /// </summary>
        /// <param name="player">The name of the player's name you want to changed</param>
        /// <param name="Name_">The name you want to change to</param>
        /// <returns>false if changing the name went worng. true if the name was updated successfully</returns>
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
                Console.WriteLine("Something went wrpng while changing the name");
                return false;
            }
        }
        /// <summary>
        /// Changes the requested player's password
        /// </summary>
        /// <param name="player">The password of the player's password you want to changed</param>
        /// <param name="Password_">The password you want to change to</param>
        /// <returns>false if changing the password went worng. true if the password was updated successfully</returns>
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
        //<summary>
        //Gets all of the question's info (including the player)
        //adds the question to the database
        //</summary>
        public void AddQuestion(string text_, string correct_, string wrong1_, string wrong2_, string wrong3_, Player player, int subject_)
        {
            try
            {
                Question question = new Question() { PlayerId=player.PlayerId, Correct=correct_, Incorrect1=wrong1_, Incorrect2 = wrong2_, Incorrect3 = wrong3_, QuestionText=text_, SubjectId=subject_, StatusId=1, };
                this.Questions.Add(question);
                player.Points = 0;
                SaveChanges();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); Console.WriteLine("Failed, please try again."); }
        }
        /// <summary>
        /// Gets a player by entered email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>returns a player with this email or null if it doesn't exist</returns>
        public Player GetPlayerByEmail(string email)
        {
            try
            {
                 return this.Players.Where(Player=>Player.Email==email).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Signs up a new player
        /// </summary>
        /// <param name="username">specified valid player</param>
        /// <param name="password">specified valid password</param>
        /// <param name="email">specified valid email</param>
        /// <returns>The signed up player</returns>
        /// <exception cref="Exception">Throws an exception if the signup failed in any way. email already exists or anything else</exception>
        public Player SignUp(string username,string password,string email)
        {
            if(GetPlayerByEmail(email)!= null)//checks if a player with this email already exists
            {
                throw new Exception("Email already exists");
            }
            Player p = new Player() {Email=email,PlayerName=username,Password=password,RankId=1,Points=0};
            try//if adding the new player fails
            {
                this.Players.Add(p);
                SaveChanges();
                return p;
            }
            catch
            {
                throw new Exception("Sign up failed");
            }
        }
        /// <summary>
        /// gets a random approved question from the database
        /// </summary>
        /// <returns>a question</returns>
        public Question GetRandomQuestion()
        {
            Random rnd = new Random();
            int questionId = rnd.Next(1, (this.Questions.Count() + 1));
            Question question = this.Questions.Where(Question => Question.QuestionId == questionId && Question.StatusId == 2).Include(q=>q.Subject).FirstOrDefault();
            while (question == null)
            {
                questionId = rnd.Next(1, (this.Questions.Count() + 1));
                question = this.Questions.Where(Question => Question.QuestionId == questionId && Question.StatusId == 2).Include(q => q.Subject).FirstOrDefault();
            }
            return question;
        }
        /// <summary>
        /// gets the first question in the database that is pending
        /// </summary>
        /// <returns>a question</returns>
        public Question GetPendingQuestion()
        {
            return this.Questions.Where(Question => Question.StatusId == 1).FirstOrDefault();
        }
        /// <summary>
        /// Changes a player's points based on if they answered correctly or not
        /// </summary>
        /// <param name="p">the player</param>
        /// <param name="b">true or false based on player's answer</param>
        public void ChangePoints(Player player, bool isCorrect)
        {
            if (isCorrect)
            {
                player.Points += 10;
                if (player.Points>100) player.Points = 100;
            }
            else
            {
                player.Points -= 5;
                if (player.Points<0) player.Points = 0;
            }
            SaveChanges();
        }
    }
}
