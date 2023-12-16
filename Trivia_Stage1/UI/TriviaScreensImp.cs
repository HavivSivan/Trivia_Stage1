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
        public bool ShowLogin()//Made by Idan
        {
            ClearScreenAndSetTitle("Login");
            LoggedPlayer = null;//Logs out logged user
            bool logged=false;//This bool is the bool that will be returned at the end of the screen depending if the login was succesful or not
            Console.Write("Please Type your email: ");
            string email = Console.ReadLine();//Gets the email from the user
            LoggedPlayer = Context.GetPlayerByEmail(email);//this method gets a string and returns a player with this email, if there isn't one it returns null
            //we can use the return of the null and try to try and put the password in
            //and if the player is null ity will throw an exception that will be caught by the try and tell the user the login failed
            Console.Write("Please Type your password: ");
            string password = Console.ReadLine();//gets the password from the user
            try
            {
                if (LoggedPlayer.Password == password) logged = true;//checks if the password of the playerByEmail is the same as teh inputed password
                //if not the loggin will fail
                //and if the player is null (player with this email does not exist or something else(why try is great!)) 
                //the login will also fail by catching the exception
                else//I thought about writing password is wrong but that will be stupid
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Login failed please try again later.");
                    Console.ResetColor();
                    Console.WriteLine("press enter to continue");
                    Console.ReadLine();
                    logged = false;
                }
            }
            catch//Here the exception is caught and the login fails
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Login failed please try again later.");
                Console.ResetColor();
                Console.WriteLine("press enter to continue");
                Console.ReadLine();
                logged = false;
            }
            return logged;
        }
        public bool ShowSignUp()//made by Idan
        {
            //Logout user if anyone is logged in!
            //A reference to the logged in user should be stored as a member variable
            //in this class! Example:
            //this.currentyPLayer == null
            LoggedPlayer = null;
            //Loop through inputs until a user/player is created or 
            //user choose to go back to menu
            bool signed=false; //this bool indicates if the signup went succesfully or not
            //Clear screen
            ClearScreenAndSetTitle("Signup");

            Console.Write("Please Type your email: ");
            string email = Console.ReadLine();//gets the email from the user
            while (!IsEmailValid(email))//checks if the email is valid
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Bad Email Format! Please try again:");
                Console.ResetColor();
                email = Console.ReadLine();
            }

            Console.Write("Please Type your password: ");
            string password = Console.ReadLine();//gets the password from the user
            while (!IsPasswordValid(password))//checks if the password is valid
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("password must be at least 4 characters! Please try again: ");
                Console.ResetColor();
                password = Console.ReadLine();
            }

            Console.Write("Please Type your Name: ");
            string name = Console.ReadLine();//gets the name from the suer
            while (!IsNameValid(name))//cehcks if the name is valid
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("name must be at least 3 characters! Please try again: ");
                Console.ResetColor();
                name = Console.ReadLine();
            }

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Connecting to Server...");
            Console.ResetColor();
            try//try is used here in case of an exception and the signup not working
            {
                this.LoggedPlayer = Context.SignUp(name, password, email);//this method adds the new player to the database and saves chenges
                //if the email already exists or any other exception happens the method throws an according exception
                //which is caught here and written for the user to know
                //if no exception is caught untill here sign up then was successful
                Console.WriteLine("Sign up succesful press enter to continue");
                Console.ReadLine();
                signed = true;//here signup was successful
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(ex.Message);//based on exception thrown by SignUp method
                Console.WriteLine("!Please try again next time");
                Console.ResetColor();
                Console.WriteLine("press enter to continue");
                Console.ReadLine();
                signed = false;//here signup was unsuccessful
            }
            return signed;
        }

        public void ShowAddQuestion()
        {
            ClearScreenAndSetTitle("Add Question");
            if (LoggedPlayer.Points == 100)//if the player has enough points
            {
                Console.WriteLine("Please type your question.");
                string text = Console.ReadLine();
                Console.WriteLine("Please write the correct answer");
                string right = Console.ReadLine();
                Console.WriteLine("Please type 3 wrong answers.");
                string wrong1 = Console.ReadLine();
                string wrong2 = Console.ReadLine();
                string wrong3 = Console.ReadLine();
                Console.WriteLine("Please type the subject of the question. (1-Sports, 2-Politics, 3-History, 4-Science, 5-Ramon");
                int num; int.TryParse(Console.ReadLine(), out num);
                //Gets all of the info for the question.
                Context.AddQuestion(text, right, wrong1, wrong2, wrong3, LoggedPlayer, num); //adds the question
                LoggedPlayer.Points=0;
                Console.WriteLine("\nQuestion submitted successfully! Press any key to continue.");
                Console.ReadKey(true);//end
            }
            else //if the player doesn't have enough points
            {
                Console.WriteLine("Sorry, you do not have enough points to submit a question. Press any key to continue.");    
                Console.ReadKey(true);//end
            }
            
            
        }

        public void ShowPendingQuestions() //made by ofek
        {
            bool isContinue = true;
            while (isContinue) //as long as user wants to keep approving questions this loops
            {
                ClearScreenAndSetTitle("Pending Questions");
                Question question = Context.GetPendingQuestion(); //gets first pending question on the list
                try
                {
                    Console.WriteLine($"Question subject: {this.Context.Subjects.Where(Subject => Subject.SubjectId == question.SubjectId).First().SubjectName}"); //prints the question's subject (if there are any pending questions remaining)
                }
                catch
                {
                    Console.WriteLine("No more questions are pending. Press any key to return to the menu");
                    Console.ReadKey(true);
                    return;
                }
                Console.WriteLine(question.QuestionText); //prints the question's text and answers in order
                Console.WriteLine($"{1}. {question.Correct} <- Correct Answer");
                Console.WriteLine($"{2}. {question.Incorrect1}");
                Console.WriteLine($"{3}. {question.Incorrect2}");
                Console.WriteLine($"{4}. {question.Incorrect3}");
                Console.WriteLine();
                Console.WriteLine("Press 'y' if you want to approve the question. Press any other key to decline this question.");
                var ch1 = Console.ReadKey();
                Console.WriteLine();     
                if ((ch1.KeyChar == 'y' || ch1.KeyChar == 'Y')) //checks and confirms if user wants to approve/decline question, and changes question status id in database accordingly
                {
                    Console.WriteLine("Are you sure you want to approve this question? Press 'y' to confirm. Press any other key to decline it instead.");
                    var ch2 = Console.ReadKey();
                    Console.WriteLine();
                    if ((ch2.KeyChar == 'y' || ch2.KeyChar == 'Y'))
                    {
                        question.StatusId = 2;
                        Context.SaveChanges();
                    }
                    else
                    {
                        question.StatusId = 3;
                        Context.SaveChanges();
                    }
                }
                else
                {
                    Console.WriteLine("Are you sure you want to decline this question? Press 'y' to confirm. Press any other key to approve it instead.");
                    var ch3 = Console.ReadKey();
                    Console.WriteLine();
                    if ((ch3.KeyChar == 'y' || ch3.KeyChar == 'Y'))
                    {
                        question.StatusId = 3;
                        Context.SaveChanges();
                    }
                    else
                    {
                        question.StatusId = 2;
                        Context.SaveChanges();
                    }
                }
                Console.WriteLine("Press 'y' if you want to continue to another pending question. Press any other button to go back to the menu.");
                var ch4 = Console.ReadKey();
                if (!(ch4.KeyChar == 'y' || ch4.KeyChar == 'Y')) //asks if user wants to keep approving questions, otherwise sends to menu
                {
                    isContinue = false;
                }
            }
            //Admin@yahoo.com
        }
        public void ShowGame() //made by ofek
        {
            bool isContinue = true;
            while (isContinue) //as long as player wants to keep playing this loops
            {
                ClearScreenAndSetTitle("Game");
                Question question = Context.GetRandomQuestion(); //pulls random approved question from the database
                try
                {
                    Console.WriteLine($"Question subject: {this.Context.Subjects.Where(Subject => Subject.SubjectId == question.SubjectId).First().SubjectName}"); //prints the question's subject
                }
                catch
                {
                    Console.WriteLine("Oops, question could not generate");
                }
                Console.WriteLine(question.QuestionText); //prints the question's text
                Random rnd = new Random();
                int q = rnd.Next(2, 5);
                int locOfCorrect = 0;
                for (int i = 0; i < 4; i++) //prints the 4 answers in a "random" order and keeps the location of the correct answer
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
                while (!int.TryParse(Console.ReadLine(), out answerLoc)) //filters inputs until player inputs an int
                {
                    Console.WriteLine("Incorrect input. Please enter a number.");
                }
                bool isCorrect = true;
                Console.WriteLine();
                if (answerLoc == locOfCorrect) //checks if int player inputted is the correct answer's location
                {
                    Console.WriteLine("Correct! :D");
                }
                else
                {
                    isCorrect = false;
                    Console.WriteLine("Incorrect :(");
                }
                Context.ChangePoints(LoggedPlayer, isCorrect); //adds or subtracts the player's points based on if they were correct or not
                Console.WriteLine($"Current points: {LoggedPlayer.Points}"); //displays the player's current points
                Console.WriteLine();
                Console.WriteLine("Press 'y' if you want to continue to another question. Press any other button to go back to the menu.");
                var ch = Console.ReadKey();
                if(!(ch.KeyChar=='y'|| ch.KeyChar=='Y')) //if player wants to keep playing they can press y, otherwise it goes back to menu
                {
                    isContinue = false;
                }
            }
            //Admin@yahoo.com
        }
        public void ShowProfile()//made by Idan
        {
            ClearScreenAndSetTitle("Profile");
            //displays all of the logged players properties(as requested)
            Console.WriteLine("Email:" + LoggedPlayer.Email);
            Console.WriteLine("Username:" + LoggedPlayer.PlayerName);
            Console.WriteLine("Total Points:"+LoggedPlayer.Points);
            //because of the ranking being a foreign key to the table of rank names ther was a need for another method which gets the rank name by the player's rankId
            Console.WriteLine("Rank:"+Context.GetRankByPlayer(LoggedPlayer));
            Console.WriteLine("Questions made:"+LoggedPlayer.QuestionsMade);
            Console.WriteLine("Password:"+LoggedPlayer.Password);
            Console.WriteLine("To change the password press p. To change username press u. To Change email press e.To exist press anything else");
            //asks the user if it wants to changed the email, password or username of their account
            char c = Console.ReadKey(true).KeyChar;
            while (c=='p'||c=='u'||c=='e')//this is a while and not an if because of the possibility that the player wants to change multiple attributes
            {
                //changing password
                if(c == 'p')
                {
                    Console.Write("Please Type your new password: ");
                    string password = Console.ReadLine();
                    //gets a password from the player and checks if it's valid or not
                    //like it was done in the sign up
                    while (!IsPasswordValid(password))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("password must be at least 4 characters! Please try again: ");
                        Console.ResetColor();
                        password = Console.ReadLine();
                    }
                    //this method is a bool that changes the logged player's password and saves changes and returns true if it was changed
                    if (Context.ChangePassword(LoggedPlayer, password))
                        
                    {
                        Console.WriteLine("Change succesful!");
                    }

                }
                //changing username
                if (c == 'u')
                {
                    Console.Write("Please Type your new Name: ");
                    string name = Console.ReadLine();
                    //gets a name from the player and checks if it's valid or not
                    //like it was done in the sign up
                    while (!IsNameValid(name))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("name must be at least 3 characters! Please try again: ");
                        Console.ResetColor();
                        name = Console.ReadLine();
                    }
                    //this method is a bool that changes the logged player's name and saves changes and returns true if it was changed
                    if (Context.ChangeName(LoggedPlayer, name))
                    {
                        Console.WriteLine("Change succesful!");
                    }
                }
                //changing email
                if (c == 'e')
                {
                    Console.Write("Please Type your new email: ");
                    string email = Console.ReadLine();
                    //gets an email from the player and checks if it's valid or not
                    //like it was done in the sign up
                    while (!IsEmailValid(email))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Bad Email Format! Please try again:");
                        Console.ResetColor();
                        email = Console.ReadLine();
                    }
                    //this method is a bool that changes the logged player's email and saves changes and returns true if it was changed
                    //also returns false if a player with this email already exists
                    if (Context.ChangeEmail(LoggedPlayer, email))
                    {
                        Console.WriteLine("Change succesful!");
                    }
                }
                //here the user has an option to change something again or exit the screen
                Console.WriteLine("To change the password press p. To change username press u. To Change email press e.To exist press anything else");
                c = Console.ReadKey(true).KeyChar;
            }
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
