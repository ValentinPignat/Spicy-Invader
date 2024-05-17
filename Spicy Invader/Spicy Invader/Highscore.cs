/// ETML
/// Author: Valentin Pignat
/// Date (creation) : 17.05.2024
/// Description : Highscore class with username and score

using System.Runtime.CompilerServices;

[assembly:InternalsVisibleToAttribute("SpicyTest")]
namespace Spicy_Invader
{
    /// <summary>
    /// Highscore class with username and score
    /// </summary>
    public class Highscore
    {
        /// <summary>
        /// Holder of the highscore
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Highscore value (score)
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Highscore constructor
        /// </summary>
        /// <param name="username">Highscore holder</param>
        /// <param name="score">Highscore value (score)</param>
        public Highscore(string username, int score) { 
            Username = username;
            Score = score;
        }
    }
}
