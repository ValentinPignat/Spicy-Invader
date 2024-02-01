/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Spicy Invader Game, initiate the player's ship and enter a loop:
///     - Reads for input 
///     - Update
///     - Thread.Sleep()

using SpaceshipNS;
using System;
using System.Threading;
using MissileNS;
using System.Windows.Input;
using System.Collections.Generic;

// Handle multiple input at each cycle 
// Assembly added: WindowsBase and PresentationCore
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard.iskeydown?view=windowsdesktop-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.key?view=windowsdesktop-8.0
// https://stackoverflow.com/questions/6373645/c-sharp-winforms-how-to-set-main-function-stathreadattribute

namespace Spicy_Invader
{
    internal class Game
    {
        // Allows Keyboard.IsKeyDown
        [STAThread]
        static void Main(string[] args)
        {
            // Game running 
            bool gameRunning = false;

            // Hides cursor
            Console.CursorVisible = false;

            // Placeholder menu
            Console.WriteLine("This is a menu");
            Console.ReadKey();
            Console.Clear();

            // Game starts
            gameRunning = true;
            
            // Create the spaceship and displays it
            SpaceShip player = new SpaceShip();

            // Loop : Reads an input, actions accordingly in player / updates missiles / sleep
            while (gameRunning) {
                player.PlayerControl();
                player.PlayerMissileUpdate();
                Thread.Sleep(100);
            }
        }   
    }
}
