/// ETML
/// Author: Valentin Pignat
/// Date (creation) : 17.05.2024
/// Description : Highscore class with username and score

namespace Spicy_Invader
{
    internal class Highscore
    {

        public string Username { get; private set; }

        public int Score { get; private set; }
        public Highscore(string username, int score) { 
            Username = username;
            Score = score;
        }
    }
}
