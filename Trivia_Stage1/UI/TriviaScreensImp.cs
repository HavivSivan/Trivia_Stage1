using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Trivia_Stage1.Models;

namespace Trivia_Stage1.UI
{
    public class TriviaScreensImp:ITriviaScreens
    {

        //Place here any state you would like to keep during the app life time
        //For example, player login details...
        public Player LoggedPlayer { get;private set; }
        private TriviaContext Context = new TriviaContext();

        //Implememnt interface here
        public bool ShowLogin()
        {
            ClearScreenAndSetTitle("Login");
            LoggedPlayer = null;
            bool logged=false;
            Console.Write("Please Type your email: ");
            string email = Console.ReadLine();
            LoggedPlayer = Context.GetPlayerByEmail(email);
            Console.Write("Please Type your password: ");
            string password = Console.ReadLine();
            try
            {
                if (LoggedPlayer.Password == password) logged = true;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Login failed please try again.");
                Console.ResetColor();
                Console.WriteLine("press enter to continue");
                Console.ReadLine();
                logged = false;
            }
            return logged;
        }
        public bool ShowSignUp()
        {
            //Logout user if anyone is logged in!
            //A reference to the logged in user should be stored as a member variable
            //in this class! Example:
            //this.currentyPLayer == null
            LoggedPlayer = null;
            //Loop through inputs until a user/player is created or 
            //user choose to go back to menu
            bool signed=false;
            //Clear screen
            ClearScreenAndSetTitle("Signup");

            Console.Write("Please Type your email: ");
            string email = Console.ReadLine();
            while (!IsEmailValid(email))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Bad Email Format! Please try again:");
                Console.ResetColor();
                email = Console.ReadLine();
            }

            Console.Write("Please Type your password: ");
            string password = Console.ReadLine();
            while (!IsPasswordValid(password))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("password must be at least 4 characters! Please try again: ");
                Console.ResetColor();
                password = Console.ReadLine();
            }

            Console.Write("Please Type your Name: ");
            string name = Console.ReadLine();
            while (!IsNameValid(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("name must be at least 3 characters! Please try again: ");
                Console.ResetColor();
                name = Console.ReadLine();
            }

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Connecting to Server...");
            Console.ResetColor();
            try
            {
                this.LoggedPlayer = Context.SignUp(name, password, email);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to signup! Please try again next time");
                Console.ResetColor();
                Console.WriteLine("press enter to continue");
                Console.ReadLine();
                signed = false;
            }
            if (LoggedPlayer != null)
            {
                Console.WriteLine("Sign up succesful press enter to continue");
                //Get another input from user
                Console.ReadLine();
                signed=true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to signup! Email already exists! Please try again next time");
                Console.ResetColor();
                Console.WriteLine("press enter to continue");
                Console.ReadLine();
                signed=false;
            }
            //return true if signup suceeded!
            return signed;
        }

        public void ShowAddQuestion()
        {
            Console.WriteLine("Not implemented yet! Press any key to continue...");
            Console.ReadKey(true);
        }

        public void ShowPendingQuestions()
        {
            Console.WriteLine("Not implemented yet! Press any key to continue...");
            Console.ReadKey(true);
        }
        public void ShowGame()
        {
            bool isContinue = true;
            while (isContinue)
            {
                ClearScreenAndSetTitle("Start Game");
                Question question = Context.GetRandomQuestion();
                try
                {
                    Console.WriteLine($"Question subject: {this.Context.Subjects.Where(Subject => Subject.SubjectId == question.SubjectId).First().SubjectName}");
                }
                catch
                {
                    Console.WriteLine("Oops, question could not generate");
                }
                Console.WriteLine(question.QuestionText);
                Random rnd = new Random();
                int q = rnd.Next(2, 5);
                int locOfCorrect = 0;
                for (int i = 0; i < 4; i++)
                {
                    switch (q)
                    {
                        case 1:
                            Console.WriteLine($"{i + 1}. {question.Correct}");
                            locOfCorrect = i + 1;
                            break;
                        case 2:
                            Console.WriteLine($"{i + 1}. {question.Incorrect1}");
                            break;
                        case 3:
                            Console.WriteLine($"{i + 1}. {question.Incorrect2}");
                            break;
                        case 4:
                            Console.WriteLine($"{i + 1}. {question.Incorrect3}");
                            break;
                    }
                    q++;
                    if (q > 4) q = 1;
                }
                Console.WriteLine();
                Console.WriteLine("Which is the correct answer? Write the number of the answer you think is correct.");
                int answerLoc = 0;
                while (!int.TryParse(Console.ReadLine(), out answerLoc))
                {
                    Console.WriteLine("Incorrect input. Please enter a number.");
                }
                bool isCorrect = true;
                Console.WriteLine();
                if (answerLoc == locOfCorrect)
                {
                    Console.WriteLine("Correct! :D");
                }
                else
                {
                    isCorrect = false;
                    Console.WriteLine("Incorrect :(");
                }
                Context.ChangePoints(LoggedPlayer, isCorrect);
                Console.WriteLine($"Current points: {LoggedPlayer.Points}");
                Console.WriteLine();
                Console.WriteLine("Press 'y' if you want to continue to another question. Press any other button to go back to the menu.");
                var ch = Console.ReadKey();
                if(!(ch.KeyChar=='y'|| ch.KeyChar=='Y'))
                {
                    isContinue = false;
                }
            }
            //Admin@yahoo.com
        }
        public void ShowProfile()
        {
            ClearScreenAndSetTitle("Profile");
            Console.WriteLine("Email:" + LoggedPlayer.Email);
            Console.WriteLine("Username:" + LoggedPlayer.PlayerName);
            Console.WriteLine("Total Points:"+LoggedPlayer.Points);
            Console.WriteLine("Rank:"+Context.GetRankByPlayer(LoggedPlayer));
            Console.WriteLine("Questions made:"+LoggedPlayer.QuestionsMade);
            Console.WriteLine("Password:"+LoggedPlayer.Password);
            Console.WriteLine("To change the password press p, To change ");
        }


        //Private helper methods down here...
        private void ClearScreenAndSetTitle(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{title,65}");
            Console.WriteLine();
            Console.ResetColor();   
        }

        private bool IsEmailValid(string emailAddress)
        {
            //regex is string based pattern to validate a text that follows a certain rules
            // see https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference

            var pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            var regex = new Regex(pattern);
            return regex.IsMatch(emailAddress);

        //another option is using .net System.Net.Mail library which has an EmailAddress class that stores email
        //we can use it to validate the structure of the email:
       // https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.mailaddress?view=net-7.0
            /*
             * try
             * {
             *     //try to create MailAddress objcect from the email address string
             *      var email=new MailAddress(emailAddress);
             *      //if success
             *      return true;
             * }
             *      //if it throws a formatExcpetion then the string is not email format.
             * catch (Exception ex)
             * {
             * return false;
             * }
             */

        }



        private bool IsPasswordValid(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length >= 3&&password.Length<=20;
        }

        private bool IsNameValid(string name)
        {
            return !string.IsNullOrEmpty(name) && name.Length >= 3&&name.Length<=30;
        }
    }
}
