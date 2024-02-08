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
        /// <summary>
        /// Margin for the display / player and ennemies movement zone
        /// </summary>
        public const int MARGIN_SIDE = 5;

        /// <summary>
        /// Margin for the display / player and ennemies movement zone
        /// </summary>
        public const int MARGIN_TOP_BOTTOM = 2;

        /// <summary>
        /// Width of the game space 
        /// </summary>
        public const int WIDTH = 50;

        /// <summary>
        /// Height of the game space 
        /// </summary>
        public const int HEIGHT = 30;

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
            DrawLayout();
            
            // Create the spaceship and displays it
            SpaceShip player = new SpaceShip(x: MARGIN_SIDE + (WIDTH/2), y : HEIGHT);
            player.Draw();

            // Create ennemy block 
            EnemyBlock enemyBlock = new EnemyBlock();
            
            
            // Loop : Reads an input, acts accordingly in player / update missiles / sleep
            while (gameRunning) {
                player.PlayerControl();
                player.PlayerMissileUpdate();
                enemyBlock.Update();
                Thread.Sleep(100);
            }
        }

        static void DrawLayout() {

            for (int i = MARGIN_SIDE; i <= WIDTH + MARGIN_SIDE; i++) {
                for (int j = MARGIN_TOP_BOTTOM; j <= HEIGHT + MARGIN_TOP_BOTTOM; j++)
                {
                    if (i == MARGIN_SIDE && j == MARGIN_TOP_BOTTOM)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write("╔");
                    }
                    else if (i == MARGIN_SIDE && j == MARGIN_TOP_BOTTOM + HEIGHT)
                    {

                        Console.SetCursorPosition(i, j);
                        Console.Write("╚");
                    }
                    else if (i == MARGIN_SIDE + WIDTH && j == MARGIN_TOP_BOTTOM + HEIGHT)
                    {

                        Console.SetCursorPosition(i, j);
                        Console.Write("╝");
                    }
                    else if (i == MARGIN_SIDE + WIDTH && j == MARGIN_TOP_BOTTOM )
                    {

                        Console.SetCursorPosition(i, j);
                        Console.Write("╗");
                    }

                    else if (i == MARGIN_SIDE || i == WIDTH + MARGIN_SIDE) {
                        Console.SetCursorPosition(i, j);
                        Console.Write("║");
                    }
                    else if (j == MARGIN_TOP_BOTTOM || j == HEIGHT + MARGIN_TOP_BOTTOM) 
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write("═");
                    }               
                }
            }
        }
    }
}
