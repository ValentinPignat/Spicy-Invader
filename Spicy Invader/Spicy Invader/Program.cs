/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Spicy Invader Game, initiate the player's ship and enter a loop:
///     - Reads for input 
///     - Update
///     - Thread.Sleep()

/// TODO :
///     - Change to onhit method in each object to manage collision and avoid updatig all ? THTHTHTHTH
///     ADD MENU
///     see cdc
///     DISABLE SCROLL AND CLICK
///     CALCULATE WIDTH AND HEIGHT FROM SPRTIE (No need for manual change)
///     ENEMY MOVE AFTER COLISION AND DEAD UPDATE ? 
///     Stop console change / remove nativemethods ?
///
/// REVIEW CDC: 26.04.2024
/// https://perso.esiee.fr/~perretb/I3FM/POO1/projet/index.html#cahier-des-charges-fonctionnel
/// 
/// Missing/Incomplete feature:
///     - Son on/off
///     
///     - Difficulté facile/difficile
///     - Highscore
///     - A propos
///     - Quitter

using SpaceshipNS;
using System;
using System.Threading;
using EnemiesNS;
using EnemyBlockNS;
using MissileNS;
using System.Collections.Generic;
using GameObjectsNS;
using BricksNS;
using System.Windows.Input;
using System.Diagnostics;




// Handle multiple input at each cycle 
// Assembly added: WindowsBase and PresentationCore
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard.iskeydown?view=windowsdesktop-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.key?view=windowsdesktop-8.0
// https://stackoverflow.com/questions/6373645/c-sharp-winforms-how-to-set-main-function-stathreadattribute

namespace Spicy_Invader
{
    internal class Program
    {
        public partial class NativeMethods
        {

            /// Return Type: BOOL->int
            ///fBlockIt: BOOL->int
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "BlockInput")]
            [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool BlockInput([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fBlockIt);

        }

        // Allows Keyboard.IsKeyDown
        [STAThread]
        static void Main(string[] args)
        {
            NativeMethods.BlockInput(true);

            Menu menu = new Menu();
            // Placeholder menu
            ConsoleSetup();
            Console.WriteLine("Space Invaders");
            while (Console.KeyAvailable) { Console.ReadKey(true); }
            Console.ReadKey();
            Game game = new Game();

            while (Console.KeyAvailable) { Console.ReadKey(true); }
            Console.WriteLine("We are in program.cs");
            Console.ReadLine(); 
        }


        /// <summary>
        /// Set up console parameters (Dimension, Cursor)
        /// </summary>
        static void ConsoleSetup()
        {
            // Set console size depending on map size 
            Console.WindowWidth = Game.WIDTH + Game.WIDTH_CONSOLE_MARGIN;
            Console.WindowHeight = Game.HEIGHT + Game.HEIGHT_CONSOLE_MARGIN;

            // Hides cursor
            Console.CursorVisible = false;
        }
    }
}
