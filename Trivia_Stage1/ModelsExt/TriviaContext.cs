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
    }
}
